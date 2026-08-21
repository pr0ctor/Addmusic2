using Addmusic2.Exceptions;
using Addmusic2.Helpers;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Parsers;
using Addmusic2.Services;
using Addmusic2.Visitors;
using Antlr4.Runtime;
using AsarCLR.Asar191;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Addmusic2.Logic
{
    internal class AddmusicLogic : IAddmusicLogic
    {
        private IAddmusicLogger _logger;
        private MessageService _messageService;
        private IFileCachingService _fileCachingService;
        private GlobalSettings _globalSettings;
        private RomOperations _romOperations;

        private List<Song> Songs = new();
        private List<SoundEffect> SoundEffects = new();

        public DateTime LastModification { get; set; }

        public int AddmusicLogicDataVersion { get; } = 4;
        public AddmusicKCheckBits CheckBits { get; } = new AddmusicKCheckBits();

        IAddmusicCheckBits IAddmusicLogic.CheckBits => CheckBits;

        public AddmusicLogic(
            IAddmusicLogger logger,
            MessageService messageService,
            IFileCachingService fileCachingService,
            IGlobalSettings globalSettings,
            RomOperations romOperations
        )
        {
            _logger = logger;
            _messageService = messageService;
            _fileCachingService = fileCachingService;
            _globalSettings = (GlobalSettings)globalSettings;
            _romOperations = romOperations;
        }

        public void Run()
        {

            _logger.LogInformation(LogLevel.Information, $"Beginning processing for Addmusic data version {AddmusicLogicDataVersion}", true);

            var loadedRom = _romOperations.LoadRomData();

            _logger.LogInformation(LogLevel.Debug, $"Loaded Rom {loadedRom.RomFileName}");

            _logger.LogInformation(LogLevel.Debug, "Cleaning existing Rom data");
            var roms = CleanRomData(loadedRom);

            LoadRequiredSampleGroups();

            _logger.LogInformation(LogLevel.Information, "Processing All Songs");

            ProcessAllSongs();

            _logger.LogInformation(LogLevel.Information, "Processing All Sound Effects");

            ProcessAllSoundEffects();

            GetProgramUploadPosition();

            AssembleSPCDriver();

            _logger.LogInformation(LogLevel.Information, "Compiling Songs and Sound Effects");

            CompileAllSoundEffects(SoundEffects);
            CompileSongs(roms.TempRom, Songs);

            _logger.LogInformation(LogLevel.Information, "Fixing Pointers");

            FixMusicPointers(Songs);

            if(_globalSettings.GenerateSPC == true)
            {
                _logger.LogInformation(LogLevel.Information, "Assembling final patch");

                AssembleFinalPatch(roms.TempRom, Songs);
                // todo double check romname logic
                GenerateMSC(roms.OrignalRom.RomFileName, Songs);
            }

            _logger.LogInformation(LogLevel.Information, "Finished processing");
        }

        #region Song Processing

        public void ProcessAllSongs()
        {
            _logger.LogInformation(LogLevel.Debug, $"Processing {_globalSettings.ResourceList.Songs.GlobalSongs.Count} Global Songs");
            // load global songs
            foreach (var globalSong in _globalSettings.ResourceList.Songs.GlobalSongs)
            {
                _logger.LogInformation(LogLevel.Trace, $"Processing Song {globalSong.Name}");
                var fileData = File.ReadAllText(Path.Combine(FileNames.FolderNames.MusicBase, globalSong.Path));

                var preprocessedFileData = PreProcessSong(fileData);

                var songData = ProcessSong(preprocessedFileData, SongScope.Global);
                songData.Configuration = globalSong;

                PostProcessSong();

                Songs.Add(songData);
                _logger.LogInformation(LogLevel.Trace, $"Finished Processing Song {globalSong.Name}");
            }

            _logger.LogInformation(LogLevel.Debug, $"Processing {_globalSettings.ResourceList.Songs.LocalSongs.Count} Local Songs");

            // load local songs
            foreach (var localSong in _globalSettings.ResourceList.Songs.LocalSongs)
            {
                _logger.LogInformation(LogLevel.Trace, $"Processing Song {localSong.Name}");
                var fileData = File.ReadAllText(Path.Combine(FileNames.FolderNames.MusicBase, localSong.Path));

                var preprocessedFileData = PreProcessSong(fileData);

                var songData = ProcessSong(preprocessedFileData, SongScope.Local);
                songData.Configuration = localSong;

                PostProcessSong();

                Songs.Add(songData);
                _logger.LogInformation(LogLevel.Trace, $"Finished Processing Song {localSong.Name}");
            }

            _logger.LogInformation(LogLevel.Debug, $"Processed {Songs.Count} Songs");
        }


        public void RunSingleSong(string fileData)
        {

        }


        // Performs various Preprocessing steps
        //      - Searches for comments and replaces them with whitespace
        //      - Searches for all instances of Replacement patterns in order to consume
        //          and replace Replacement patterns within the song file
        //      - Searches for instances of specific preprocessor directives in order
        //          to pass through an separate preprocessor grammar step
        public string PreProcessSong(string fileData)
        {
            // Normalize line endings for the entire file
            fileData = fileData.ReplaceLineEndings("\n");

            // Replace all comments with empty string since comments break up some tokens during the main processing
            fileData = Regexes.LineCommentRegex().Replace(fileData, string.Empty);

            var replacementMatches = Regexes.ReplacementParameterRegex().Matches(fileData);


            _logger.LogInformation(LogLevel.Trace, $"Found {replacementMatches.Count} replacement matches");

            foreach (Match match in replacementMatches)
            {
                var searchValue = match.Groups[1].Value;
                var replaceValue = match.Groups[2].Value;

                _logger.LogInformation(LogLevel.Trace, $"Found search value [ {searchValue} ] => Replacing with [ {replaceValue} ]");

                fileData = fileData.Replace(searchValue, replaceValue);
            }

            var validPreprocessorIndicators = new List<string>()
            {
                "#if",
                "#else",
                "#elseif",
                "#endif",
                "#define",
                "#undef",
                "#ifdef",
                "#ifndef",
                "#error",
            };

            var preprocessorMatches = Regex.Match(fileData, $"({string.Join("|", validPreprocessorIndicators)})+");

            if( preprocessorMatches.Success && preprocessorMatches.Length > 0 )
            {
                // run preprocessor logic and get final parsed filedata
            }


            return fileData;
        }

        public Song ProcessSong(string fileData, SongScope songScope)
        {
            var stream = CharStreams.fromString(fileData);

            var lexer = new MmlLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new MmlParser(tokenStream);
            var mmlVisitor = new AdvMmlVisitor();

            var songContext = parser.song();

            var rootNode = mmlVisitor.VisitSong(songContext);

            // potentially logic for changing parsers

            var songParser = new SongParser(
                _logger,
                _messageService,
                _globalSettings,
                _fileCachingService,
                songScope
            );

            var song = new Song(songParser, rootNode)
            {
                SongText = fileData,
            };

            song.ParseSong();

            return song;
        }

        public void PostProcessSong()
        {

        }

        #endregion


        #region Sound Effect Processing

        public void ProcessAllSoundEffects()
        {
            _logger.LogInformation(LogLevel.Debug, $"Processing {_globalSettings.ResourceList.SoundEffects.Sfx1DF9.Count} 1DF9 SFX");
            // load 1DF9 sound effects
            foreach (var sfx1DF9 in _globalSettings.ResourceList.SoundEffects.Sfx1DF9)
            {
                // dont do anything if this is a sound effect that points to another
                if (sfx1DF9.Settings.Pointer == true)
                {
                    SoundEffects.Add(new SoundEffect
                    {
                        Configuration = sfx1DF9
                    });
                    continue;
                }

                //var filedata = File.ReadAllText(Path.Combine(FileNames.FolderNames.Sfx1DF9, sfx1DF9.Path));
                var filedata = File.ReadAllText(Path.Combine(sfx1DF9.Path));
                filedata = filedata.Trim();

                var preprocessedSfxFileData = PreprocessSoundEffect(filedata);

                var soundEffectData = ProcessSoundEffect(sfx1DF9, preprocessedSfxFileData);
                soundEffectData.Configuration = sfx1DF9;

                PostProcessSoundEffect();

                SoundEffects.Add(soundEffectData);
            }

            _logger.LogInformation(LogLevel.Debug, $"Processing {_globalSettings.ResourceList.SoundEffects.Sfx1DFC.Count} 1DFC SFX");
            // load 1DFC sound effects
            foreach (var sfx1DFC in _globalSettings.ResourceList.SoundEffects.Sfx1DFC)
            {
                // dont do anything if this is a sound effect that points to another
                if (sfx1DFC.Settings.Pointer == true)
                {
                    SoundEffects.Add(new SoundEffect
                    {
                        Configuration = sfx1DFC
                    });
                    continue;
                }

                //var filedata = File.ReadAllText(Path.Combine(FileNames.FolderNames.Sfx1DFC, sfx1DFC.Path));
                var filedata = File.ReadAllText(Path.Combine(sfx1DFC.Path));

                var preprocessedSfxFileData = PreprocessSoundEffect(filedata);

                var soundEffectData = ProcessSoundEffect(sfx1DFC, preprocessedSfxFileData);
                soundEffectData.Configuration = sfx1DFC;

                PostProcessSoundEffect();

                SoundEffects.Add(soundEffectData);
            }
        }


        public string PreprocessSoundEffect(string fileData)
        {
            return fileData;
        }

        public SoundEffect ProcessSoundEffect(SfxListItem sfxItem, string fileData)
        {
            var stream = CharStreams.fromString(fileData);

            var lexer = new SfxLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SfxParser(tokenStream);
            var sfxVisitor = new AdvSfxVisitor();

            var soundEffectContext = parser.soundEffect();

            var rootNode = sfxVisitor.VisitSoundEffect(soundEffectContext);

            var soundEffectParser = new SoundEffectParser(
                _logger,
                _messageService,
                _globalSettings,
                _fileCachingService,
                _romOperations,
                sfxItem
            );

            var soundEffect = new SoundEffect(soundEffectParser, rootNode)
            {
                SoundEffectText = fileData,
            };

            soundEffect.ParseSoundEffect();

            return soundEffect;
        }

        public void PostProcessSoundEffect()
        {

        }


        #endregion


        #region Addmusic Core Logic

        public (Rom OrignalRom, Rom TempRom) CleanRomData(Rom rom)
        {
            var tempRom = rom.CreateTempRomCopy();
            tempRom.RomFileName = "temp";
            tempRom.RomFilePath = FileNames.SfcFiles.TempRomSfc;
            tempRom.RomFileExtension = FileNames.FileExtensions.RomSfc;

            // Check if the rom is clean
            //      if clean rom, return current rom and copy of current rom, no need to clean
            //      else clean the rom data
            if (rom.RomData[CheckBits.CleanRomFirstCheckBitLocation] == CheckBits.CleanRomFirstCheckBitValue
                && rom.RomData[CheckBits.CleanRomSecondCheckBitLocation] == CheckBits.CleanRomSecondCheckBitValue)
            {
                _logger.LogInformation(LogLevel.Information, _messageService.GetNotificationCurrentRomIsCleanRomMessage(), true);
                return (rom, tempRom);
            }

            // double check locations
            var programNameRaw = tempRom.RomData.GetRange(_romOperations.SNESToPC(CheckBits.AmkCheckValueLocation), CheckBits.AmkCheckValueLength);
            var programNameString = new String(programNameRaw.Select(x => (char)x).ToArray());

            _logger.LogInformation(LogLevel.Trace, $"Found Addmusic Program Name '{programNameString}'");

            if (programNameString != CheckBits.AmkCheckValueStringValue)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetNotificationRomAmkVersionCannotBeDeterminedMessage(programNameString, CheckBits.AmkCheckValueStringValue), true);
                throw new InvalidAddmusicVersionException(_messageService.GetNotificationRomAmkVersionCannotBeDeterminedMessage(programNameString, CheckBits.AmkCheckValueStringValue));
            }

            var romDataVersion = tempRom.RomData[_romOperations.SNESToPC(CheckBits.AmkDataVersionLocation)];
            _logger.LogInformation(LogLevel.Trace, $"Found Rom Data Version {romDataVersion}");

            if (romDataVersion > AddmusicLogicDataVersion)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetNotificationRomAmkDataVersionMismatchMessage(romDataVersion.ToString()), true);
                throw new InvalidAddmusicVersionException(_messageService.GetNotificationRomAmkDataVersionMismatchMessage(romDataVersion.ToString()));
            }

            var samplesNumbersListAddress = _romOperations.SNESToPC(MagicNumbers.SamplesNumbersListAddress & 0xFFFFFF);
            _romOperations.ClearRATSTag(ref tempRom, samplesNumbersListAddress - 8);

            var songSamplePointersAddress = _romOperations.SNESToPC(MagicNumbers.SongSamplePointersAddress);
            var eraseSamples = false;

            var index = 0;
            while(true)
            {
                samplesNumbersListAddress = (songSamplePointersAddress + (index * 3)) & 0xFFFFFF;

                if(samplesNumbersListAddress == 0xFFFFFF)
                {
                    if(eraseSamples == false)
                    {
                        eraseSamples = true;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if(samplesNumbersListAddress != 0)
                    {
                        _romOperations.ClearRATSTag(ref tempRom, samplesNumbersListAddress - 8);
                        _logger.LogInformation(LogLevel.Trace, $"Cleared RATS tag at {samplesNumbersListAddress - 8}");
                    }
                }

                index++;
            }

            tempRom.WriteRomDataToFile();

            return (rom, tempRom);
        }

        public void GetProgramUploadPosition()
        {
            var patchAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.PatchAsmName) 
                ?? throw new FileNotFoundException(_messageService.GetErrorRequiredFileNotFoundMessage(FileNames.AsmFiles.PatchAsmName, FileNames.AsmFiles.PatchAsmPath));
            var patchAsmStream = Encoding.UTF8.GetString(patchAsm.ToArray());
            //var patchAsmStream = Encoding.Unicode.GetString(patchAsm.ToArray());
            var programUploadPositionRegex = Helpers.Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramUploadPositionText);
            var matches = programUploadPositionRegex.Matches(patchAsmStream);
            // If no matches are found, then that value is missing from the expected file
            if (matches.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorProgramUploadPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramUploadPositionText, FileNames.AsmFiles.PatchAsmPath), true);
                throw new MissingFileDataException(_messageService.GetErrorProgramUploadPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramUploadPositionText, FileNames.AsmFiles.PatchAsmPath));
            }

            // get the base16 value for the position
            var value = matches.First().Groups[1].Value;

            // convert it to an int from base16
            var intValue = Convert.ToInt32(value, 16);
            _globalSettings.ProgramUploadPosition = intValue;
        }

        public void AssembleSPCDriver()
        {
            if (File.Exists(FileNames.BinFiles.MainBin))
            {
                File.Delete(FileNames.BinFiles.MainBin);
            }

            var mainAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.MainAsmName)
                ?? throw new FileNotFoundException(_messageService.GetErrorRequiredFileNotFoundMessage(FileNames.AsmFiles.MainAsmName, FileNames.AsmFiles.MainAsmPath));

            var mainAsmText = Encoding.UTF8.GetString(mainAsm.ToArray());

            var complied = _romOperations.CompileAsmToBin(FileNames.AsmFiles.MainAsmPath, FileNames.BinFiles.MainBin);

            if (!complied)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorAsarErrorOccurredMessage(), true);
                throw new AsarExecutionException(_messageService.GetErrorAsarErrorOccurredMessage());
            }

            var tempTextFile = File.ReadAllText(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempTextFile));

            var programPostionRegex = Helpers.Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramBasePositionText);
            var mainLoopPositionRegex = Helpers.Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText);
            var reuploadPositionRegex = Helpers.Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ReuploadPositionText);

            var mainBaseMatches = programPostionRegex.Matches(mainAsmText);
            var mainLoopMatches = mainLoopPositionRegex.Matches(tempTextFile);
            var reuploadMatches = reuploadPositionRegex.Matches(tempTextFile);

            // Cannot find Program Base Position from the file
            if (mainBaseMatches.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorProgramBasePositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText, FileNames.StaticFiles.TempTextFile), true);
                throw new MissingFileDataException(_messageService.GetErrorProgramBasePositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText, FileNames.StaticFiles.TempTextFile));
            }

            // Cannot find Main Loop Position from the file
            if (mainLoopMatches.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorMainLoopPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText, FileNames.StaticFiles.TempTextFile), true);
                throw new MissingFileDataException(_messageService.GetErrorMainLoopPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText, FileNames.StaticFiles.TempTextFile));
            }
            // Cannot find Reupload Position from the file
            if (reuploadMatches.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorReuploadPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.ReuploadPositionText, FileNames.StaticFiles.TempTextFile), true);
                throw new MissingFileDataException(_messageService.GetErrorReuploadPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.ReuploadPositionText, FileNames.StaticFiles.TempTextFile));
            }

            var noSFXIsFound = tempTextFile.Contains(ExtractedAsmDataNames.AdditionalValues.NoSFXIsEnabled, StringComparison.CurrentCulture);

            if (_globalSettings.ExportSfx == true && noSFXIsFound == false)
            {
                _logger.LogWarning(LogLevel.Information, _messageService.GetWarningNoSfxEnabledAndDumpSfxMessage(), true);
                _globalSettings.ExportSfx = false;
            }

            var fileInfo = new FileInfo(FileNames.BinFiles.MainBin).Length;
            _globalSettings.ProgramSize = (int)fileInfo;
            _globalSettings.ProgramBasePosition = Convert.ToInt32(mainBaseMatches.First().Groups[1].Value.Replace("$", ""), 16);
            _globalSettings.MainLoopPosition = Convert.ToInt32(mainLoopMatches.First().Groups[1].Value.Replace("$", ""), 16);
            _globalSettings.ProgramReuploadPosition = Convert.ToInt32(reuploadMatches.First().Groups[1].Value.Replace("$", ""), 16);

        }

        public void CompileAllSoundEffects(List<SoundEffect> soundEffects)
        {
            var sfx1DF9 = soundEffects.FindAll(s => s.Configuration.Type == SfxListItemType.Sfx1DF9);
            var sfx1DFC = soundEffects.FindAll(s => s.Configuration.Type == SfxListItemType.Sfx1DFC);

            var sfx1DF9Max = sfx1DF9.Max(s => s.Configuration.IntNumber);
            var missing1DF9 = Enumerable.Range(0, sfx1DF9Max).Except(sfx1DF9.Select(s => s.Configuration.IntNumber));

            var sfx1DFCMax = sfx1DFC.Max(s => s.Configuration.IntNumber);
            var missing1DFC = Enumerable.Range(0, sfx1DFCMax).Except(sfx1DFC.Select(s => s.Configuration.IntNumber));

            var df9Pointers = new List<ushort>();
            var df9DataTotal = 0;
            var dfcPointers = new List<ushort>();
            var dfcDataTotal = 0;

            var allSfxData = new List<byte>();

            var index = 0;
            foreach (var sfx in sfx1DF9)
            {
                // fill in blanks between items
                while (missing1DF9.Contains(index))
                {
                    df9Pointers.Add(0xFFFF);
                    index++;
                }

                // if the current sound effect points to another existing sound effect
                //      get the pointer for that sound effect
                //      otherwise calculate and store the data for the sound effect
                if (sfx.Configuration.Settings.Pointer == true)
                {
                    df9Pointers.Add(df9Pointers[sfx.Configuration.Settings.CopyOfIntNumber - 1]);
                }
                else
                {
                    // Calculate AramPosition because that was not done during the Parsing of the Sound Effect
                    sfx.SoundEffectData.AramPosition = sfx1DF9Max * 2
                        + sfx1DFCMax * 2
                        + _globalSettings.ProgramBasePosition
                        + _globalSettings.ProgramSize
                        + df9DataTotal;
                    // Compile the Asm Elements now that there is a defined AramPosition
                    // todo handle errors
                    sfx.Parser.CompileAsmElements(sfx.SoundEffectData);

                    var pointer = dfcDataTotal
                        + df9DataTotal
                        + (sfx1DF9Max + sfx1DFCMax) * 2
                        + _globalSettings.ProgramBasePosition
                        + _globalSettings.ProgramSize;

                    df9Pointers.Add((ushort)pointer);
                    df9DataTotal += (sfx.SoundEffectData.ChannelData.Count + sfx.SoundEffectData.CompiledAsmCodeBlocks.Count);

                    allSfxData.AddRange(sfx.SoundEffectData.ChannelData);
                    foreach (var item in sfx.SoundEffectData.CompiledAsmCodeBlocks.Values)
                    {
                        allSfxData.AddRange(item);
                    }
                }

                index++;
            }

            index = 0;
            foreach (var sfx in sfx1DFC)
            {
                // fill in blanks between items
                while (missing1DFC.Contains(index))
                {
                    dfcPointers.Add(0xFFFF);
                    index++;
                }

                // if the current sound effect points to another existing sound effect
                //      get the pointer for that sound effect
                //      otherwise calculate and store the data for the sound effect
                if (sfx.Configuration.Settings.Pointer == true)
                {
                    dfcPointers.Add(dfcPointers[sfx.Configuration.Settings.CopyOfIntNumber - 1]);
                }
                else
                {
                    // Calculate AramPosition because that was not done during the Parsing of the Sound Effect
                    sfx.SoundEffectData.AramPosition = sfx1DF9Max * 2
                        + sfx1DFCMax * 2
                        + _globalSettings.ProgramBasePosition
                        + _globalSettings.ProgramSize
                        + dfcDataTotal;
                    // Compile the Asm Elements now that there is a defined AramPosition
                    // todo handle errors
                    sfx.Parser.CompileAsmElements(sfx.SoundEffectData);

                    var pointer = dfcDataTotal
                        + df9DataTotal
                        + (sfx1DF9Max + sfx1DFCMax) * 2
                        + _globalSettings.ProgramBasePosition
                        + _globalSettings.ProgramSize;

                    dfcPointers.Add((ushort)pointer);
                    dfcDataTotal += (sfx.SoundEffectData.ChannelData.Count + sfx.SoundEffectData.CompiledAsmCodeBlocks.Count);

                    allSfxData.AddRange(sfx.SoundEffectData.ChannelData);
                    foreach (var item in sfx.SoundEffectData.CompiledAsmCodeBlocks.Values)
                    {
                        allSfxData.AddRange(item);
                    }
                }

                index++;
            }

            var df9Size = (df9DataTotal + (sfx1DF9Max * 2));
            var dfcSize = (dfcDataTotal + (sfx1DFCMax * 2));
            var allSize = df9Size + dfcSize;
            _logger.LogInformation(LogLevel.Information, _messageService.GetInfoTotalSpaceUsedBy1DF9SfxMessage($"0x{df9Size:X4}"));
            _logger.LogInformation(LogLevel.Information, _messageService.GetInfoTotalSpaceUsedBy1DFCSfxMessage($"0x{dfcSize:X4}"));
            _logger.LogInformation(LogLevel.Information, _messageService.GetInfoTotalSpaceUsedByAllSoundEffectsMessage($"0x{allSize:X4}"));


            PatchBuilders.WriteDataToBinFile(df9Pointers.ToArray(), FileNames.BinFiles.Sfx1DF9TableBin);
            PatchBuilders.WriteDataToBinFile(dfcPointers.ToArray(), FileNames.BinFiles.Sfx1DFCTableBin);
            PatchBuilders.WriteDataToBinFile(allSfxData.ToArray(), FileNames.BinFiles.SfxDataBin);
            //File.WriteAllBytes(FileNames.BinFiles.SfxDataBin, allSfxData.ToArray());

            var mainAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.MainAsmName)
                ?? throw new FileNotFoundException(_messageService.GetErrorRequiredFileNotFoundMessage(FileNames.AsmFiles.MainAsmName, FileNames.AsmFiles.MainAsmPath));

            mainAsm.Seek(index, SeekOrigin.Begin);

            var mainAsmText = Encoding.UTF8.GetString(mainAsm.ToArray());
            var sfxTable0Position = mainAsmText.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable0Text);
            mainAsmText = mainAsmText.Insert(
                sfxTable0Position + ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable0Text.Length,
                PatchBuilders.SfxTable0Contents
            );

            var sfxTable1Position = mainAsmText.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable1Text);
            mainAsmText = mainAsmText.Insert(
                sfxTable1Position + ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable1Text.Length,
                PatchBuilders.SfxTable1Contents
            );

            File.WriteAllText(FileNames.AsmFiles.TempMainAsmPath, mainAsmText);

            File.Delete(FileNames.BinFiles.MainBin);

            var isCompiled = _romOperations.CompileAsmToBin(FileNames.AsmFiles.TempMainAsmPath, FileNames.BinFiles.MainBin);

            if (!isCompiled)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorAsarErrorOccurredMessage(), true);
                throw new AsarExecutionException(_messageService.GetErrorAsarErrorOccurredMessage());
            }


            var newProgramSize = (int)(new FileInfo(FileNames.BinFiles.MainBin).Length);

            if (_globalSettings.ExportSfx == true)
            {
                _messageService.GetInfoSoundEffectsNotIncludedMessage();
                _messageService.GetInfoTotalSizeOfProgramMessage($"0x{newProgramSize:X4}");
            }
            else
            {
                _messageService.GetInfoTotalSizeOfProgramWithSfxMessage($"0x{newProgramSize:X4}");
            }

            _globalSettings.ProgramSize = newProgramSize;

        }

        public void CompileSongs(Rom rom, List<Song> songs)
        {
            var highestGlobalSongNumber = _globalSettings.ResourceList.Songs.GlobalSongs.Max(s => s.IntNumber);
            var totalSampleCount = 0;
            var maxGlobalEchoBufferSize = 0;

            var songsNumberMax = songs.Max(s => s.Configuration.IntNumber);
            var missingSongs = Enumerable.Range(0, songsNumberMax).Except(songs.Select(s => s.Configuration.IntNumber));

            foreach (var song in songs)
            {

                if (song.Configuration.IntNumber > highestGlobalSongNumber)
                {
                    song.SongData.EchoBufferSize = Math.Max(song.SongData.EchoBufferSize, maxGlobalEchoBufferSize);
                }

                song.Parser.CalculateFirstPassPointers(song.SongData);

                if (song.Configuration.IntNumber <= highestGlobalSongNumber)
                {
                    maxGlobalEchoBufferSize = Math.Max(song.SongData.EchoBufferSize, maxGlobalEchoBufferSize);
                }

                totalSampleCount += song.SongData.SampleInstrumentManager.UsedSamples.Count;
            }

            var songPointerListBuilder = new StringBuilder();
            var samplePointerListBuilder = new StringBuilder();

            songPointerListBuilder.Append(PatchBuilders.SongSampleListXkasOverride);
            songPointerListBuilder.Append(PatchBuilders.SongSampleGroupPointerLabel);

            var songSampleListSize = MagicNumbers.DefaultValues.InitialSongSampleListLength;


            // todo refactor this:
            //      batch 16 songs in "dw SGPointer00, SGPointer01" format
            //      when a song is missing use $0000 instead of the associated SGPointer##

            //var songIndices = songs.Select(s => new (s.Configuration.Number,  s.Configuration.IntNumber, s.Configuration.));

            for (int i = 0; i < songsNumberMax; i++)
            {
                var missingCurrentIndex = missingSongs.Contains(i);
                var song = (missingCurrentIndex) ? null : songs.First(s => s.Configuration.IntNumber == i);
                if (i % 16 == 0)
                {
                    songPointerListBuilder.Append("\ndw ");
                }
                if (missingCurrentIndex)
                {
                    songPointerListBuilder.Append("$0000");
                }
                else
                {
                    songPointerListBuilder.Append(PatchBuilders.SongListPointerName(i.ToString("X2")));
                }

                songSampleListSize += 2;

                if (i < songsNumberMax && (i % 15 == 0))
                {
                    songPointerListBuilder.Append(", ");
                }

                // skip samples if the current song is missing
                if (missingCurrentIndex || song == null)
                {
                    continue;
                }

                songSampleListSize++;

                samplePointerListBuilder.Append($"\n{PatchBuilders.SongListPointerName(i.ToString("X2"))}:\n");

                if (i > highestGlobalSongNumber)
                {
                    continue;
                }
                var numberOfSamples = song.SongData.SampleInstrumentManager.UsedSamples.Count;
                songSampleListSize += numberOfSamples * 2;
                samplePointerListBuilder.Append($"db ${numberOfSamples}\n");

                var sampleLengths = song.SongData.SampleInstrumentManager.UsedSamples.Select(s =>
                {
                    return $"${s.Data.Count:X4}";
                });

                samplePointerListBuilder.Append($"{string.Join(",", sampleLengths)}\n");

            }
            var songSampleListAsmBuilder = new StringBuilder();
            var freespaceLocation = _romOperations.FindFreeSpaceInROM(rom, songSampleListSize, _globalSettings.BankStart);
            var freespaceSNESValue = _romOperations.PCToSNES(freespaceLocation);
            songSampleListAsmBuilder.Append($"org ${freespaceSNESValue:X6}\n\n");
            songSampleListAsmBuilder.Append(songPointerListBuilder.ToString());
            songSampleListAsmBuilder.Append("\n\n");
            songSampleListAsmBuilder.Append(samplePointerListBuilder.ToString());
            songSampleListAsmBuilder.Append($"\n{PatchBuilders.SongSampleListEndLabel}");

            File.WriteAllText(FileNames.AsmFiles.SongSampleListAsmPath, songSampleListAsmBuilder.ToString());
        }

        public void FixMusicPointers(List<Song> songs)
        {

            var pointerPosition = _globalSettings.ProgramSize + 0x400;
            var highestGlobalSong = songs.Max(s => s.Configuration.IntNumber);
            var songDataAramPosition = _globalSettings.ProgramSize + _globalSettings.ProgramUploadPosition + (highestGlobalSong * 2) + 2;

            var globalPointersBuilder = new StringBuilder();
            var localPointersBuilder = new StringBuilder();
            var incbinsBuilder = new StringBuilder();

            var atLocalSongs = false;

            foreach (Song song in songs.OrderBy(s => s.Configuration.IntNumber))
            {
                var songData = song.SongData;

                songData.PositionInARAM = songDataAramPosition;

                var untilJump = -1;

                if (songData.SongScope == SongScope.Global)
                {
                    globalPointersBuilder.Append($"\ndw song{song.Configuration.IntNumber:X2}");
                    incbinsBuilder.Append($"song{song.Configuration.IntNumber:X2}: incbin \"{FileNames.FolderNames.AsmSNES}/{FileNames.FolderNames.AsmSNESBin}/music{song.Configuration.IntNumber:X2}{FileNames.FileExtensions.BinPatchData}");
                }
                if (atLocalSongs == false && songData.SongScope == SongScope.Local)
                {
                    localPointersBuilder.Append("\ndw localSong");
                    incbinsBuilder.Append("localSong: ");
                    atLocalSongs = true;
                }

                for (int i = 0; i < songData.SpaceForPointersAndInstruments; i += 2)
                {

                    if (untilJump == 0)
                    {
                        i += songData.SampleInstrumentManager.GetTotalInstrumentSpace();
                        untilJump = -1;
                    }

                    var temp = songData.AllPointersAndInstruments[i] | songData.AllPointersAndInstruments[i + 1] << 8;

                    if (temp == 0xFFFF) // 0xFFFF = swap with 0x0000.
                    {
                        songData.AllPointersAndInstruments[i] = 0;
                        songData.AllPointersAndInstruments[i + 1] = 0;
                        untilJump = 1;
                    }
                    else if (temp == 0xFFFE) // 0xFFFE = swap with 0x00FF.
                    {
                        songData.AllPointersAndInstruments[i] = 0xFF;
                        songData.AllPointersAndInstruments[i + 1] = 0;
                        untilJump = 2;
                    }
                    else if (temp == 0xFFFD) // 0xFFFD = swap with the song's position (its first track pointer).
                    {
                        songData.AllPointersAndInstruments[i] = (byte)((songData.PositionInARAM + 2) & 0xFF);
                        songData.AllPointersAndInstruments[i + 1] = (byte)((songData.PositionInARAM + 2) >> 8);
                    }
                    else if (temp == 0xFFFC) // 0xFFFC = swap with the song's position + 2 (its second track pointer).
                    {
                        songData.AllPointersAndInstruments[i] = (byte)(songData.PositionInARAM & 0xFF);
                        songData.AllPointersAndInstruments[i + 1] = (byte)(songData.PositionInARAM >> 8);
                    }
                    else if (temp == 0xFFFB) // 0xFFFB = swap with 0x0000, but don't set untilSkip.
                    {
                        songData.AllPointersAndInstruments[i] = 0;
                        songData.AllPointersAndInstruments[i + 1] = 0;
                    }
                    else
                    {
                        temp += songData.PositionInARAM;
                        songData.AllPointersAndInstruments[i] = (byte)(temp & 0xFF);
                        songData.AllPointersAndInstruments[i + 1] = (byte)(temp >> 8);
                    }

                    untilJump--;
                }

                var totalChannelSizes = songData.ChannelData.Sum(c => c.ChannelData.Count);
                var combinedChannelData = new List<byte>();

                foreach (var channel in songData.ChannelData)
                {
                    foreach (var location in channel.LoopLocations)
                    {
                        var temp = (channel.ChannelData[location] & 0xFF) | (channel.ChannelData[location + 1] << 8);
                        temp += songData.PositionInARAM + totalChannelSizes + songData.SpaceForPointersAndInstruments;
                        channel.ChannelData[location] = (byte)(temp & 0xFF);
                        channel.ChannelData[location + 1] = (byte)(temp >> 8);
                    }
                    combinedChannelData.AddRange(channel.ChannelData);
                }

                var songRatsData = new List<byte>();
                var finalSongBinData = new List<byte>();

                var sizePadding = (songData.MinSize > 0) ? songData.MinSize : songData.TotalSize;

                if (song.Configuration.IntNumber > highestGlobalSong)
                {
                    var ratsSize = songData.TotalSize + 4 - 1;

                    songRatsData.AddRange(MagicNumbers.StarTag);

                    songRatsData.Add((byte)(ratsSize & 0xFF));
                    songRatsData.Add((byte)(ratsSize >> 8));

                    songRatsData.Add((byte)(~ratsSize & 0xFF));
                    songRatsData.Add((byte)(~ratsSize >> 8));

                    songRatsData.Add((byte)(sizePadding & 0xFF));
                    songRatsData.Add((byte)(sizePadding >> 8));

                    songRatsData.Add((byte)(songDataAramPosition & 0xFF));
                    songRatsData.Add((byte)(songDataAramPosition >> 8));

                }

                finalSongBinData.AddRange(songData.AllPointersAndInstruments);
                finalSongBinData.AddRange(combinedChannelData);
                if (songData.MinSize > 0 && song.Configuration.IntNumber <= highestGlobalSong)
                {
                    var remainingSize = (songRatsData.Count + finalSongBinData.Count) - songData.MinSize;
                    if (remainingSize < 0)
                    {
                        finalSongBinData.AddRange(Enumerable.Repeat<byte>(0, Math.Abs(remainingSize)));
                    }
                }
                songData.RatsData = songRatsData;
                songData.FinalData = finalSongBinData;

                var filePath = FileNames.BinFiles.FinalMusicDataBin($"music{song.Configuration.IntNumber:X2}{FileNames.FileExtensions.BinPatchData}");
                var writeableData = new List<byte>();
                writeableData.AddRange(songData.RatsData);
                writeableData.AddRange(songData.FinalData);
                File.WriteAllBytes(filePath, writeableData.ToArray());

                if (songData.SongScope == SongScope.Global)
                {
                    songDataAramPosition += sizePadding;
                }
                else if (songData.SongScope == SongScope.Local && _globalSettings.EnableEchoCheck)
                {
                    var spaceInfo = songData.SpaceInfo;
                    var samples = songData.SampleInstrumentManager.UsedSamples;
                    spaceInfo.ImportantSampleCount = samples.Where(s => s.IsImportant == true).ToList().Count;
                    spaceInfo.SongStartPosition = songDataAramPosition;
                    spaceInfo.SongEndPosition = songDataAramPosition + sizePadding;

                    var checkPosition = songDataAramPosition + sizePadding;
                    if ((checkPosition & 0xFF) != 0)
                    {
                        checkPosition = ((checkPosition >> 8) + 1) << 8;
                    }
                    spaceInfo.SampleTableStartPosition = checkPosition;


                    foreach (var sample in samples)
                    {
                        // todo check how to determine duplcaites

                        var duplicate = false;
                        if (!duplicate)
                        {
                            var startPosition = checkPosition;
                            var endPosition = checkPosition + sample.Data.Count;
                            spaceInfo.SamplePositions.Add(sample, (startPosition, endPosition));

                            checkPosition += sample.Data.Count;
                        }

                    }

                    var endOfSongSampleDataPosition = checkPosition;

                    if (checkPosition > 0x10000)
                    {
                        // todo handle error for Sample Data space usage limit reached
                        throw new Exception();
                    }

                    if ((checkPosition & 0xFF) != 0)
                    {
                        checkPosition = ((checkPosition >> 8) + 1) << 8;
                    }

                    checkPosition += songData.EchoBufferSize << 11;

                    spaceInfo.EchoBufferStartPosition = (songData.EchoBufferSize > 0)
                        ? 0x10000 - (songData.EchoBufferSize << 11)
                        : 0xFF00;
                    spaceInfo.EchoBufferEndPosition = (songData.EchoBufferSize > 0)
                        ? 0x10000
                        : 0xFF04;

                    if (checkPosition > 0x10000)
                    {
                        // todo handle error for EchoBuffer Data space usage limit reached
                        throw new Exception();
                    }
                }

            }

            var finalTempAsmBuilder = new StringBuilder();
            var tempAsm = File.ReadAllText(FileNames.AsmFiles.TempMainAsmPath);

            finalTempAsmBuilder.Append(tempAsm);
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(globalPointersBuilder.ToString());
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(localPointersBuilder.ToString());
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(incbinsBuilder.ToString());

            File.WriteAllText(FileNames.AsmFiles.TempMainAsmPath, finalTempAsmBuilder.ToString());

            var isCompiled = _romOperations.CompileAsmToBin(FileNames.AsmFiles.TempMainAsmPath, FileNames.BinFiles.MainSongDataBin);

            if (!isCompiled)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorAsarErrorOccurredMessage(), true);
                throw new AsarExecutionException(_messageService.GetErrorAsarErrorOccurredMessage());
            }

            var compiledFileSize = (int)(new FileInfo(FileNames.BinFiles.MainSongDataBin).Length);
            _globalSettings.ProgramSize = compiledFileSize;

            var postCompileFileData = new List<byte>();
            postCompileFileData.AddRange(new List<byte>
            {
                (byte)(compiledFileSize & 0xFF),
                (byte)(compiledFileSize >> 8),
                (byte)(_globalSettings.ProgramUploadPosition & 0xFF),
                (byte)(_globalSettings.ProgramUploadPosition >> 8),
            });
            postCompileFileData.AddRange(File.ReadAllBytes(FileNames.BinFiles.MainSongDataBin));

            File.WriteAllBytes(FileNames.BinFiles.MainSongDataBin, postCompileFileData.ToArray());

            if (_globalSettings.Verbose == true)
            {
                // todo add message for the completion of this step
            }

            // no need to handle the bank defines portion
        }

        public void AssembleFinalPatch(Rom rom, List<Song> songs)
        {

            var patchData = File.ReadAllText(FileNames.AsmFiles.PatchAsmPath);
            var replacePatchData = Helpers.Helpers.SetHexValueAfterText(patchData, ExtractedAsmDataNames.PatchAsmLocationNames.ExpARAMRetText, $"{_globalSettings.ProgramReuploadPosition:X4}");
            replacePatchData = Helpers.Helpers.SetHexValueAfterText(replacePatchData, ExtractedAsmDataNames.PatchAsmLocationNames.DefARAMRetText, $"{_globalSettings.MainLoopPosition:X4}");
            replacePatchData = Helpers.Helpers.SetHexValueAfterText(replacePatchData, ExtractedAsmDataNames.PatchAsmLocationNames.SongCountText, $"{songs.Count:X2}");

            var musicPointersLocation = replacePatchData.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.MusicPointersText);
            // Cannot find MusicPointersText from the file
            if (musicPointersLocation == -1)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorMusicPointersTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MusicPointersText, FileNames.AsmFiles.PatchAsmPath), true);
                throw new MissingFileDataException(_messageService.GetErrorMainLoopPositionTextMissingMessage(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText, FileNames.AsmFiles.PatchAsmPath));
            }
            var subPatchBuilder = new StringBuilder();
            subPatchBuilder.Append(replacePatchData[..musicPointersLocation]);

            var musicPointerBuilder = new StringBuilder();
            musicPointerBuilder.Append("MusicPtrs: \ndl ");
            var samplePointerBuilder = new StringBuilder();
            samplePointerBuilder.Append("\n\nSamplePtrs:\ndl ");
            var sampleLoopPointerBuilder = new StringBuilder();
            sampleLoopPointerBuilder.Append("\n\nSampleLoopPtrs:\ndw ");
            var musicIncBinsPointerBuilder = new StringBuilder();
            musicIncBinsPointerBuilder.Append("\n\n");
            var sampleIncBinsPointerBuilder = new StringBuilder();
            sampleIncBinsPointerBuilder.Append("\n\n");

            var songCount = 0;
            var usedSamples = new List<Sample>();
            foreach (var song in songs)
            {
                if (song.SongData.SongScope == SongScope.Local)
                {
                    var fileName = FileNames.BinFiles.FinalMusicDataBin($"music{song.Configuration.IntNumber:X2}{FileNames.FileExtensions.BinPatchData}");
                    var songDataFileSize = (int)(new FileInfo(fileName).Length);
                    var freespace = _romOperations.FindFreeSpaceInROM(rom, songDataFileSize, _globalSettings.BankStart);

                    if (freespace == -1)
                    {
                        // todo handle exception
                        throw new Exception();
                    }

                    var snesFreespace = _romOperations.PCToSNES(freespace);

                    musicPointerBuilder.Append($"music{song.Configuration.IntNumber:X2}+8");
                    musicIncBinsPointerBuilder.Append(PatchBuilders.MusicIncBinBuilder(freespace, song.Configuration.IntNumber));
                }
                else
                {
                    musicPointerBuilder.Append($"${0:X6}");
                }

                if ((songCount % 16) == 0 && songCount != songs.Count - 1)
                {
                    musicPointerBuilder.Append("\ndl ");
                }
                else
                {
                    musicPointerBuilder.Append(", ");
                }

                usedSamples.AddRange(song.SongData.SampleInstrumentManager.UsedSamples);

                songCount++;
            }

            var sampleCount = 0;
            foreach (var sample in usedSamples)
            {
                var sampleData = new List<byte>();
                sampleData.AddRange(MagicNumbers.StarTag);
                sampleData.Add((byte)((sample.Data.Count + 1) & 0xFF));
                sampleData.Add((byte)((sample.Data.Count + 1) >> 8));
                sampleData.Add((byte)(~(sample.Data.Count + 1) & 0xFF));
                sampleData.Add((byte)(~(sample.Data.Count + 1) >> 8));
                sampleData.Add((byte)(sample.Data.Count & 0xFF));
                sampleData.Add((byte)(sample.Data.Count >> 8));

                var sampleDataStream = _fileCachingService.GetFromCache(sample.Name);
                sampleData.AddRange(sampleData.ToArray());
                var sampleFileName = FileNames.BinFiles.FinalSampleBrrDataBin($"brr{sampleCount}{FileNames.FileExtensions.BinPatchData}");
                File.WriteAllBytes(sampleFileName, sampleData.ToArray());
                var brrBinFileSize = (int)(new FileInfo(sampleFileName).Length);

                var freespace = _romOperations.FindFreeSpaceInROM(rom, brrBinFileSize, _globalSettings.BankStart);

                if (freespace == -1)
                {
                    // todo handle exception
                    throw new Exception();
                }
                var snesFreespace = _romOperations.PCToSNES(freespace);

                samplePointerBuilder.Append($"brr{sampleCount:X2}+8");
                sampleIncBinsPointerBuilder.Append(PatchBuilders.SampleBrrIncBinBuilder(freespace, sampleCount));


                sampleLoopPointerBuilder.Append($"${sample.LoopPoint:X4}");

                if (sampleCount % 16 == 0 && sampleCount != usedSamples.Count - 1)
                {
                    samplePointerBuilder.Append("\ndl ");
                    sampleLoopPointerBuilder.Append("\ndw ");
                }
                else
                {
                    samplePointerBuilder.Append(", ");
                    sampleLoopPointerBuilder.Append(", ");
                }

                sampleCount++;
            }

            subPatchBuilder.Append("pullpc\n\n");
            musicPointerBuilder.Append("\ndl $FFFFFF\n");
            samplePointerBuilder.Append("\ndl $FFFFFF\n");

            subPatchBuilder.Append(musicPointerBuilder.ToString());
            subPatchBuilder.Append(samplePointerBuilder.ToString());
            subPatchBuilder.Append(sampleLoopPointerBuilder.ToString());
            subPatchBuilder.Append(musicIncBinsPointerBuilder.ToString());
            subPatchBuilder.Append(sampleIncBinsPointerBuilder.ToString());

            subPatchBuilder.Append(PatchBuilders.FinalDataPatchSpcProgramLocation);

            var finalSubPatch = Helpers.Helpers.SetHexValueAfterText(
                subPatchBuilder.ToString(),
                ExtractedAsmDataNames.PatchAsmLocationNames.GlobalMusicCountText,
                $"{_globalSettings.ResourceList.Songs.GlobalSongs.Max(s => s.IntNumber):X2}"
            );

            if (File.Exists(FileNames.SfcFiles.TempPatchSfc))
            {
                File.Delete(FileNames.SfcFiles.TempPatchSfc);
            }

            var amUndo = File.ReadAllText(FileNames.AsmFiles.AMUndoAsmPath);

            File.WriteAllText(FileNames.AsmFiles.TempFinalPatchName, amUndo + finalSubPatch.ToString());

            if (_globalSettings.Verbose)
            {
                // todo handle verbosity
            }

            if (_globalSettings.GeneratePatches == false)
            {
                var isPatched = _romOperations.PatchAsmToRom(FileNames.AsmFiles.TempFinalPatchName, FileNames.SfcFiles.TempPatchSfc);

                if (!isPatched)
                {
                    _logger.LogError(LogLevel.Critical, _messageService.GetErrorAsarErrorOccurredMessage(), true);
                    throw new AsarExecutionException(_messageService.GetErrorAsarErrorOccurredMessage());
                }

                var patchedRomData = File.ReadAllBytes(FileNames.SfcFiles.TempPatchSfc);

                // todo fix and manage ROM lifecycle
                var finalRomData = new List<byte>();

                var romName = "" + "~" + FileNames.FileExtensions.OldFile;

                File.Delete(romName);
                File.WriteAllBytes(romName, finalRomData.ToArray());

                // todo fix this section
            }
        }

        public void GenerateMSC(string romName, List<Song> songs)
        {
            var mscName = $"{romName[..romName.LastIndexOf(".")]}{FileNames.FileExtensions.MscFile}";
            _logger.LogInformation(LogLevel.Debug, $"Generating {mscName} with {songs.Count} song{((songs.Count == 1) ? "" : "s")}");
            var mscBuilder = new StringBuilder();
            foreach (var song in songs)
            {
                mscBuilder.Append($"{song.Configuration.IntNumber:X2}\t0\t{song.Configuration.Name}\n");
                mscBuilder.Append($"{song.Configuration.IntNumber:X2}\t1\t{song.Configuration.Name}\n");
            }
            File.WriteAllText(mscName, mscBuilder.ToString());
        }



        #endregion


        #region Helpers

        private void LoadRequiredSampleGroups()
        {
            _logger.LogInformation(LogLevel.Debug, "Loading required Sample Groups '#default' and '#optimized'");
            var sampleGroups = _globalSettings.ResourceList.SampleGroups;

            var defaultGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesDefault, StringComparison.InvariantCultureIgnoreCase));
            var optimizedGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesOptimized, StringComparison.InvariantCultureIgnoreCase));

            // Error if there are multiple Sample Groups named '#default'
            if(defaultGroup.Count > 1)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorFoundDuplicateDefaultSampleGroupsMessage(), true);
                throw new Exception(_messageService.GetErrorFoundDuplicateDefaultSampleGroupsMessage());
            }
            // Error if there are no Sample Groups named '#default'
            else if(defaultGroup.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorMissingDefaultSampleGroupMessage(), true);
                throw new Exception(_messageService.GetErrorMissingDefaultSampleGroupMessage());
            }

            // Error if there are multiple Sample Groups named '#optimized'
            if (optimizedGroup.Count > 1)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorFoundDuplicateOptimizedSampleGroupsMessage(), true);
                throw new Exception(_messageService.GetErrorFoundDuplicateOptimizedSampleGroupsMessage());
            }
            // Error if there are no Sample Groups named '#optimized'
            else if (optimizedGroup.Count == 0)
            {
                _logger.LogError(LogLevel.Critical, _messageService.GetErrorMissingOptimizedSampleGroupMessage(), true);
                throw new Exception(_messageService.GetErrorMissingOptimizedSampleGroupMessage());
            }

            Helpers.Helpers.LoadSampleGroupToCache(_logger, _fileCachingService, defaultGroup.First());
            Helpers.Helpers.LoadSampleGroupToCache(_logger, _fileCachingService, optimizedGroup.First());
        }

        #endregion
    }
}
