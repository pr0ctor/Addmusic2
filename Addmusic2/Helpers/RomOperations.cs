using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Services;
using AsarCLR.Asar191;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Helpers
{
    internal class RomOperations : IRomOperations
    {
        private ILogger<IAddmusicLogic> _logger;
        private GlobalSettings _globalSettings;
        private MessageService _messageService;
        private FileCachingService _fileCachingService;
        //private List<byte> RomData = new();

        public RomOperations(IGlobalSettings settings, ILogger<IAddmusicLogic> logger, MessageService messageService, IFileCachingService fileCachingService)
        {
            _globalSettings = (GlobalSettings)settings;
            _messageService = messageService;
            _logger = logger;
            _fileCachingService = (FileCachingService)fileCachingService;

        }

        public Rom LoadRomData()
        {
            var romPath = "";

            // catches full filepath or name is in the current location that the program can pull from
            if (File.Exists(_globalSettings.RomName))
            {
                romPath = _globalSettings.RomName;
            }
            // check to see if the rom is either in the Install Location or in the Execution Location
            //      and if its not then the file data from the rom cannot be determined or the name is wrong
            else
            {
                var executionLocationRom = Path.Combine(FileNames.ExecutionLocations.ExecutionLocation, _globalSettings.RomName);
                var installLocationRom = Path.Combine(FileNames.ExecutionLocations.InstallLocation, _globalSettings.RomName);
                if (File.Exists(installLocationRom))
                {
                    romPath = installLocationRom;
                }
                else if (File.Exists(executionLocationRom))
                {
                    romPath = executionLocationRom;
                }
                else
                {
                    // todo fix exception message
                    throw new FileNotFoundException();
                }
            }

            // Fix cases of mixed slashes and standardize them for the current operation system file structure
            var standardizedPath = Helpers.StandardizeFileDirectoryDelimiters(romPath);
            var lastDirectorySeparator = (standardizedPath.Contains(@"\"))
                ? standardizedPath.LastIndexOf(@"\")
                : (standardizedPath.Contains(@"/"))
                    ? standardizedPath.LastIndexOf(@"/")
                    : 0;
            var romName = standardizedPath[lastDirectorySeparator..];
            var romInfo = new FileInfo(standardizedPath);
            var rom = new Rom(_messageService, this)
            {
                RomFileName = romInfo.Name,
                RomFilePath = romInfo.FullName,
                RomFileExtension = romInfo.Extension,
                RomFileSize = romInfo.Length,
                AllowSA1 = _globalSettings.EnableSA1Addressing,
            };

            rom.LoadRomData();

            return rom;
        }

        public Rom LoadRomData(string romPath)
        {
            var romInfo = new FileInfo(romPath);
            // var romData = File.ReadAllBytes(romPath);
            var rom = new Rom(_messageService, this)
            {
                RomFileName = romInfo.Name,
                RomFilePath = romInfo.FullName,
                RomFileExtension = romInfo.Extension,
                RomFileSize = romInfo.Length,
                AllowSA1 = _globalSettings.EnableSA1Addressing,
            };

            rom.LoadRomData();

            //RomData.AddRange(romData);
            return rom;
        }

        public int FindFreeSpaceInROM(Rom rom, int size, int start)
        {
            if(rom.RomData.Count == 0)
            {
                throw new Exception("Rom Data not loaded.");
            }

            if(size == 0)
            {
                // todo handle case where size cannot be 0
                throw new ArgumentException();
            }

            if(size > MagicNumbers.FourKiBRomSize)
            {
                // todo handle case where size cannot be larger than 4KiB
                throw new ArgumentException();
            }

            var position = 0;
            var space = 0;
            size += 8;
            var index = start;
            for(index = index; index < rom.RomData.Count; index++)
            {
                if(space == size)
                {
                    position = index;
                    break;
                }

                if(index % 0x8000 == 0)
                {
                    space = 0;
                }

                // Check for the start of a RATS tag
                var ratsCheckSpan = new ReadOnlySpan<byte>(rom.RomData.GetRange(index, 4).ToArray());
                if(index < rom.RomData.Count - 4 && ratsCheckSpan.SequenceEqual(MagicNumbers.StarTag))
                {
                    var ratsSize = rom.RomData[index + 4] | rom.RomData[index + 5] << 8;
                    var sizeInv = (rom.RomData[index + 6] | rom.RomData[index + 7] << 8) ^ 0xFFFF;

                    // If theres a size mismatch or if theres technically a sequence match but its not a RATS tag
                    //      continue;
                    // Otherwise
                    //      skip from the current position to the end of the protected RATS section
                    if(ratsSize != sizeInv)
                    {
                        space++;
                        continue;
                    }

                    index = index + ratsSize + 8;
                    space = 0;
                }
                else if (rom.RomData[index] == 0 || _globalSettings.EnableAggressiveFreespace == true)
                {
                    space++;
                }
                else
                {
                    space = 0;
                }
            }

            if(space == size)
            {
                position = index;
            }

            if(position == 0)
            {
                if(start == 0x080000)
                {
                    return -1;
                }
                else
                {
                    return FindFreeSpaceInROM(rom, size, 0x080000);
                }
            }

            var insertPosition = position - size;

            var bytesToInsert = new List<byte>();
            bytesToInsert.AddRange(MagicNumbers.StarTag);
            bytesToInsert.AddRange(GenerateRatsSizeValue(size - 9));
            rom.RomData.RemoveRange(insertPosition, bytesToInsert.Count);
            rom.RomData.InsertRange(insertPosition, bytesToInsert);

            return position;
        }

        public byte[] GenerateRatsSizeValue(int size)
        {
            var sizeOffset = size ^ 0xFFFF;
            return [
                (byte)(size & 0xFF),
                (byte)(size >> 8),
                (byte)(sizeOffset & 0xFF),
                (byte)(sizeOffset >> 8),
            ]; 
        }

        public int SNESToPC(int address)
        {
            return convertSNESToPC(address, _globalSettings.EnableSA1Addressing);
        }

        public int SNESToPC(int address, bool useSA1)
        {
            return convertSNESToPC(address, useSA1);
        }

        private int convertSNESToPC(int address, bool useSA1)
        {
            if (address < 0 || address > 0xFFFFFF ||     // not 24bit
                (address & 0xFE0000) == 0x7E0000 ||     // wram
                (address & 0x408000) == 0x000000)     // hardward registers
            {
                return -1;
            }

            if (useSA1 && address >= 0x808000)
            {
                address -= 0x400000;
            }

            address = ((address & 0x7F0000) >> 1 | (address & 0x7FFF));

            return address;
        }

        public int PCToSNES(int address)
        {
            return convertPCToSNES(address, _globalSettings.EnableSA1Addressing);
        }

        public int PCToSNES(int address, bool useSA1)
        {
            return convertPCToSNES(address, useSA1);
        }

        private int convertPCToSNES(int address, bool useSA1)
        {
            if (address < 0 || address >= 0x400000)
            {
                return -1;
            }

            address = ((address << 1) & 0x7F0000) | (address & 0x7FFF) | 0x8000;

            if (useSA1 && (address & 0xF00000) == 0x700000)
            {
                address |= 0x800000;
            }

            if (useSA1 && address >= 0x400000)
            {
                address += 0x400000;
            }

            return address;
        }

        public bool CompileAsmToBin(string sourceFileName, string binToWrite)
        {

            using var tempTextFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempTextFile), true);
            using var tempLogFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempLogFile), true);
            var messageBuilder = new StringBuilder();

            var dataOutArray = new byte[MagicNumbers.AsmToBinBufferLength];
            var warningSettings = new Dictionary<string, bool>()
            {
                { MagicNumbers.AsarWarnings.RelativePathWarning.WarningName, MagicNumbers.AsarWarnings.RelativePathWarning.WarningToggle }
            };
            var isPatchSuccessful = Asar.patch(sourceFileName,
                ref dataOutArray,
                default,
                default,
                default,
                default,
                default,
                warningSettings,
                default,
                default
            );

            var notifications = Asar.getprints();
            var warnings = Asar.getwarnings();
            var errors = Asar.geterrors();

            foreach ( var notification in notifications )
            {
                messageBuilder.AppendLine( notification.ToString() );
            }

            if (notifications.Length > 0)
            {
                tempTextFileWriter.WriteLine(messageBuilder.ToString());
                messageBuilder.Clear();
            }

            // todo improve logging
            messageBuilder.AppendLine("Warnings:");
            foreach ( var warning in warnings )
            {
                messageBuilder.AppendLine( warning.Fullerrdata );
            }
            messageBuilder.AppendLine("Errors:");
            foreach (var warning in warnings)
            {
                messageBuilder.AppendLine(warning.Fullerrdata);
            }

            if(warnings.Length > 0 || errors.Length > 0)
            {
                tempLogFileWriter.WriteLine(messageBuilder.ToString());
                return false;
            }
            using var binFile = File.Open(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempBinFile), FileMode.OpenOrCreate);
            binFile.Write(dataOutArray);
            binFile.Flush();
            return true;
        }

        public bool PatchAsmToRom(string sourceFileName, string romToPatch)
        {
            using var tempTextFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempTextFile), true);
            using var tempLogFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempLogFile), true);
            var messageBuilder = new StringBuilder();

            var romBytes = File.ReadAllBytes(romToPatch);

            var warningSettings = new Dictionary<string, bool>()
            {
                { MagicNumbers.AsarWarnings.RelativePathWarning.WarningName, MagicNumbers.AsarWarnings.RelativePathWarning.WarningToggle }
            };

            var isPatchSuccessful = Asar.patch(sourceFileName,
                ref romBytes,
                default,
                default,
                default,
                default,
                default,
                warningSettings,
                default,
                default
            );

            var notifications = Asar.getprints();
            var warnings = Asar.getwarnings();
            var errors = Asar.geterrors();

            foreach (var notification in notifications)
            {
                messageBuilder.AppendLine(notification.ToString());
            }

            if (notifications.Length > 0)
            {
                tempTextFileWriter.WriteLine(messageBuilder.ToString());
                messageBuilder.Clear();
            }

            // todo improve logging
            messageBuilder.AppendLine("Warnings:");
            foreach (var warning in warnings)
            {
                messageBuilder.AppendLine(warning.Fullerrdata);
            }
            messageBuilder.AppendLine("Errors:");
            foreach (var warning in warnings)
            {
                messageBuilder.AppendLine(warning.Fullerrdata);
            }

            if (warnings.Length > 0 || errors.Length > 0)
            {
                tempLogFileWriter.WriteLine(messageBuilder.ToString());
                return false;
            }

            using var sfcFile = File.Open(FileNames.SfcFiles.TempPatchSfc, FileMode.OpenOrCreate);
            sfcFile.Write(romBytes);
            sfcFile.Flush();
            return true;
        }

        public void GetProgramUploadPosition()
        {
            var patchAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.PatchAsm);
            var patchAsmStream = Encoding.Unicode.GetString(patchAsm.ToArray());
            var programUploadPositionRegex = Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramUploadPositionText);
            var matches = programUploadPositionRegex.Matches(patchAsmStream);
            if(matches.Count == 0 )
            {
                // todo catch exception when the data is missing from the file
                throw new Exception();
            }

            // get the base16 value for the position
            var value = matches.First().Groups[1].Value;

            var intValue = Convert.ToInt32(value, 16);
            _globalSettings.ProgramUploadPosition = intValue;
        }

        public void AssembleSPCDriver()
        {
            using var tempLogFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempLogFile), true);

            if (File.Exists(FileNames.BinFiles.MainBin))
            {
                File.Delete(FileNames.BinFiles.MainBin);
            }

            var mainAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.MainAsm);

            var mainAsmText = Encoding.Unicode.GetString(mainAsm.ToArray());
            var programPostion = Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ProgramBasePositionText);

            var complied = CompileAsmToBin(FileNames.AsmFiles.MainAsm, FileNames.BinFiles.MainBin);

            if(!complied)
            {
                // todo handle error
                throw new Exception();
            }

            var tempTextFile = File.ReadAllText(FileNames.StaticFiles.TempTextFile);

            var mainLoopPositionRegex = Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.MainLoopPositionText);
            var reuploadPositionRegex = Helpers.GetHexValueAfterText(ExtractedAsmDataNames.PatchAsmLocationNames.ReuploadPositionText);

            var mainLoopMatches = mainLoopPositionRegex.Matches(tempTextFile);
            var reuploadMatches = reuploadPositionRegex.Matches(tempTextFile);

            if(mainLoopMatches.Count == 0)
            {
                // todo handle error
                throw new Exception();
            }
            if(reuploadMatches.Count == 0)
            {
                // todo handle error
                throw new Exception();
            }

            var noSFXIsFound = tempTextFile.IndexOf(ExtractedAsmDataNames.AdditionalValues.NoSFXIsEnabled) != -1;

            if(_globalSettings.ExportSfx == true && noSFXIsFound == false)
            {
                // todo fix logging
                _messageService.GetWarningNoSfxEnabledAndDumpSfxMessage();
                _globalSettings.ExportSfx = false;
            }

            var fileInfo = new FileInfo(FileNames.BinFiles.MainBin).Length;
            _globalSettings.ProgramSize = (int)fileInfo;

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

            var index = 1;
            foreach (var sfx in sfx1DF9)
            {
                // fill in blanks between items
                while (missing1DF9.Contains(index))
                {
                    df9Pointers.Add(0xFFFF);
                    index++;
                }

                if (sfx.Configuration.Settings.Pointer == true)
                {
                    // get the first occurance of sound effect that the current one is pointing to
                    var copyOf = sfx1DF9
                        .FindAll(s => s.Configuration.Name == sfx.Configuration.Settings.CopyOf && s.Configuration.Settings.Pointer == false)
                        .MinBy(s => s.Configuration.IntNumber);
                    if (copyOf == null)
                    {
                        // todo fix error when theres no match
                        throw new Exception();
                    }
                    else if(copyOf.Configuration.IntNumber > sfx.Configuration.IntNumber)
                    {
                        // todo handle error when the pointer points to a sound effect that hasnt been compiled yet
                        throw new Exception();
                    }

                    // readd the pointer for this sound effect
                    df9Pointers.Add(df9Pointers[copyOf.Configuration.IntNumber]);

                }
                else
                {
                    // Calculate AramPosition because that was not done during the Parsing of the Sound Effect
                    sfx.SoundEffectData.AramPosition = sfx1DF9Max * 2
                        + sfx1DFCMax * 2
                        + _globalSettings.ProgramUploadPosition
                        + _globalSettings.ProgramSize
                        + df9DataTotal;
                    // Compile the Asm Elements now that there is a defined AramPosition
                    // todo handle errors
                    sfx.Parser.CompileAsmElements(sfx.SoundEffectData);

                    var pointer = dfcDataTotal
                        + df9DataTotal
                        + (sfx1DF9Max + sfx1DFCMax) * 2
                        + _globalSettings.ProgramUploadPosition
                        + _globalSettings.ProgramSize;

                    df9Pointers.Add((ushort)pointer);
                    df9DataTotal += sfx.SoundEffectData.ChannelData.Count + sfx.SoundEffectData.CompiledAsmCodeBlocks.Count;

                    allSfxData.AddRange(sfx.SoundEffectData.ChannelData);
                    foreach (var item in sfx.SoundEffectData.CompiledAsmCodeBlocks.Values)
                    {
                        allSfxData.AddRange(item);
                    }
                }

                index++;
            }

            index = 1;
            foreach (var sfx in sfx1DFC)
            {
                // fill in blanks between items
                while (missing1DFC.Contains(index))
                {
                    dfcPointers.Add(0xFFFF);
                    index++;
                }

                if (sfx.Configuration.Settings.Pointer == true)
                {
                    // get the first occurance of sound effect that the current one is pointing to
                    var copyOf = sfx1DFC
                        .FindAll(s => s.Configuration.Name == sfx.Configuration.Settings.CopyOf && s.Configuration.Settings.Pointer == false)
                        .MinBy(s => s.Configuration.IntNumber);
                    if (copyOf == null)
                    {
                        // todo fix error when theres no match
                        throw new Exception();
                    }
                    else if (copyOf.Configuration.IntNumber > sfx.Configuration.IntNumber)
                    {
                        // todo handle error when the pointer points to a sound effect that hasnt been compiled yet
                        throw new Exception();
                    }

                    // readd the pointer for this sound effect
                    dfcPointers.Add(dfcPointers[copyOf.Configuration.IntNumber]);

                }
                else
                {
                    // Calculate AramPosition because that was not done during the Parsing of the Sound Effect
                    sfx.SoundEffectData.AramPosition = sfx1DF9Max * 2
                        + sfx1DFCMax * 2
                        + _globalSettings.ProgramUploadPosition
                        + _globalSettings.ProgramSize
                        + dfcDataTotal;
                    // Compile the Asm Elements now that there is a defined AramPosition
                    // todo handle errors
                    sfx.Parser.CompileAsmElements(sfx.SoundEffectData);

                    var pointer = dfcDataTotal
                        + df9DataTotal
                        + (sfx1DF9Max + sfx1DFCMax) * 2
                        + _globalSettings.ProgramUploadPosition
                        + _globalSettings.ProgramSize;

                    dfcPointers.Add((ushort)pointer);
                    dfcDataTotal += sfx.SoundEffectData.ChannelData.Count + sfx.SoundEffectData.CompiledAsmCodeBlocks.Count;

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
            _messageService.GetInfoTotalSpaceUsedBy1DF9SfxMessage($"0x{PatchBuilders.HexWidthFormat(df9DataTotal.ToString(), 4)}");
            _messageService.GetInfoTotalSpaceUsedBy1DFCSfxMessage($"0x{PatchBuilders.HexWidthFormat(df9DataTotal.ToString(), 4)}");
            _messageService.GetInfoTotalSpaceUsedByAllSoundEffectsMessage($"0x{PatchBuilders.HexWidthFormat(allSize.ToString(), 4)}");



            File.WriteAllBytes(FileNames.BinFiles.SfxDataBin, allSfxData.ToArray());

            var mainAsm = _fileCachingService.GetFromCache(FileNames.AsmFiles.MainAsm);
            mainAsm.Seek(index, SeekOrigin.Begin);

            var mainAsmText = Encoding.UTF8.GetString(mainAsm.ToArray());
            var sfxTable0Position = mainAsmText.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable0Text);
            mainAsmText.Insert(
                sfxTable0Position + ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable0Text.Length,
                PatchBuilders.SfxTable0Contents
            );

            var sfxTable1Position = mainAsmText.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable1Text);
            mainAsmText.Insert(
                sfxTable0Position + ExtractedAsmDataNames.PatchAsmLocationNames.SFXTable1Text.Length,
                PatchBuilders.SfxTable1Contents
            );

            File.WriteAllText(FileNames.AsmFiles.TempMainAsm, mainAsmText);

            File.Delete(FileNames.BinFiles.MainBin);

            var isCompiled = CompileAsmToBin(FileNames.AsmFiles.TempMainAsm, FileNames.BinFiles.MainBin);

            // todo handle when it fails to compile


            var newProgramSize = (int)(new FileInfo(FileNames.BinFiles.MainBin).Length);

            if(_globalSettings.ExportSfx == true)
            {
                _messageService.GetInfoSoundEffectsNotIncludedMessage();
                _messageService.GetInfoTotalSizeOfProgramMessage($"0x{PatchBuilders.HexWidthFormat(newProgramSize.ToString(), 4)}");
            }
            else
            {
                _messageService.GetInfoTotalSizeOfProgramWithSfxMessage($"0x{PatchBuilders.HexWidthFormat(newProgramSize.ToString(), 4)}");
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

                if(song.Configuration.IntNumber > highestGlobalSongNumber)
                {
                    song.SongData.EchoBufferSize = Math.Max(song.SongData.EchoBufferSize, maxGlobalEchoBufferSize);
                }

                song.Parser.CalculateFirstPassPointers(song.SongData);
                
                if(song.Configuration.IntNumber <= highestGlobalSongNumber)
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

            for(int i = 0; i < songsNumberMax; i++)
            {
                var missingCurrentIndex = missingSongs.Contains(i);
                var song = (missingCurrentIndex) ? null : songs.First(s => s.Configuration.IntNumber == i);
                if(i % 16 == 0)
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

                if(i < songsNumberMax && (i % 15 == 0))
                {
                    songPointerListBuilder.Append(", ");
                }

                // skip samples if the current song is missing
                if (missingCurrentIndex)
                {
                    continue;
                }

                songSampleListSize++;

                samplePointerListBuilder.Append($"\n{ PatchBuilders.SongListPointerName(i.ToString("X2")) }:\n");

                if(i > highestGlobalSongNumber)
                {
                    continue;
                }
                var numberOfSamples = song.SongData.SampleInstrumentManager.UsedSamples.Count;
                songSampleListSize += numberOfSamples * 2;
                samplePointerListBuilder.Append($"db ${numberOfSamples}\n");

                var sampleLengths = song.SongData.SampleInstrumentManager.UsedSamples.Select(s =>
                {
                    return $"${Helpers.GetSampleDataLengthFromCache(_fileCachingService, s):X4}";
                });

                samplePointerListBuilder.Append($"{string.Join(",", sampleLengths)}\n");

            }
            var songSampleListAsmBuilder = new StringBuilder();
            var freespaceLocation = FindFreeSpaceInROM(rom, songSampleListSize, _globalSettings.BankStart);
            var freespaceSNESValue = PCToSNES(freespaceLocation);
            songSampleListAsmBuilder.Append($"org ${freespaceSNESValue:X6}\n\n");
            songSampleListAsmBuilder.Append(songPointerListBuilder.ToString());
            songSampleListAsmBuilder.Append("\n\n");
            songSampleListAsmBuilder.Append(samplePointerListBuilder.ToString());
            songSampleListAsmBuilder.Append($"\n{PatchBuilders.SongSampleListEndLabel}");

            File.WriteAllText(FileNames.AsmFiles.SongSampleListAsm, songSampleListAsmBuilder.ToString());
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

            foreach(Song song in songs.OrderBy(s => s.Configuration.IntNumber))
            {
                var songData = song.SongData;

                songData.PositionInARAM = songDataAramPosition;

                var untilJump = -1;

                if(songData.SongScope == SongScope.Global)
                {
                    globalPointersBuilder.Append($"\ndw song{song.Configuration.IntNumber:X2}");
                    incbinsBuilder.Append($"song{song.Configuration.IntNumber:X2}: incbin \"{FileNames.FolderNames.AsmSNES}/{FileNames.FolderNames.AsmSNESBin}/music{song.Configuration.IntNumber:X2}{FileNames.FileExtensions.BinPatchData}");
                }
                if(atLocalSongs == false && songData.SongScope == SongScope.Local)
                {
                    localPointersBuilder.Append("\ndw localSong");
                    incbinsBuilder.Append("localSong: ");
                    atLocalSongs = true;
                }

                for(int i = 0; i < songData.SpaceForPointersAndInstruments; i += 2)
                {

                    if(untilJump == 0)
                    {
                        i += songData.SampleInstrumentManager.GetTotalInstrumentSpace();
                        untilJump = -1;
                    }

                    var temp = songData.AllPointersAndInstruments[i] | songData.AllPointersAndInstruments[i + 1] << 8;

                    if(temp == 0xFFFF) // 0xFFFF = swap with 0x0000.
                    {
                        songData.AllPointersAndInstruments[i] = 0;
                        songData.AllPointersAndInstruments[i + 1] = 0;
                        untilJump = 1;
                    }
                    else if(temp == 0xFFFE) // 0xFFFE = swap with 0x00FF.
                    {
                        songData.AllPointersAndInstruments[i] = 0xFF;
                        songData.AllPointersAndInstruments[i + 1] = 0;
                        untilJump = 2;
                    }
                    else if(temp == 0xFFFD) // 0xFFFD = swap with the song's position (its first track pointer).
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

                foreach(var channel in songData.ChannelData)
                {
                    foreach(var location in channel.LoopLocations)
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

                if(song.Configuration.IntNumber > highestGlobalSong)
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
                if(songData.MinSize > 0 && song.Configuration.IntNumber <= highestGlobalSong)
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

                if(songData.SongScope == SongScope.Global)
                {
                    songDataAramPosition += sizePadding;
                }
                else if(songData.SongScope == SongScope.Local && _globalSettings.EnableEchoCheck)
                {
                    var spaceInfo = songData.SpaceInfo;
                    var samples = songData.SampleInstrumentManager.UsedSamples;
                    spaceInfo.ImportantSampleCount = samples.Where(s => s.IsImportant == true).ToList().Count;
                    spaceInfo.SongStartPosition = songDataAramPosition;
                    spaceInfo.SongEndPosition = songDataAramPosition + sizePadding;

                    var checkPosition = songDataAramPosition + sizePadding;
                    if((checkPosition & 0xFF) != 0)
                    {
                        checkPosition = ((checkPosition >> 8) + 1) << 8;
                    }
                    spaceInfo.SampleTableStartPosition = checkPosition;

                    
                    foreach(var sample in samples)
                    {
                        // check how to determine duplcaites

                        var duplicate = false;
                        if(!duplicate)
                        {
                            var startPosition = checkPosition;
                            var endPosition = checkPosition + sample.SampleDataSize;
                            spaceInfo.SamplePositions.Add(sample, (startPosition, endPosition));

                            checkPosition += sample.SampleDataSize;
                        }

                    }

                    var endOfSongSampleDataPosition = checkPosition;

                    if(checkPosition > 0x10000)
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
            var tempAsm = File.ReadAllText(FileNames.AsmFiles.TempMainAsm);

            finalTempAsmBuilder.Append(tempAsm);
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(globalPointersBuilder.ToString());
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(localPointersBuilder.ToString());
            finalTempAsmBuilder.Append('\n');
            finalTempAsmBuilder.Append(incbinsBuilder.ToString());

            File.WriteAllText(FileNames.AsmFiles.TempMainAsm, finalTempAsmBuilder.ToString());

            var isCompiled = CompileAsmToBin(FileNames.AsmFiles.TempMainAsm, FileNames.BinFiles.MainSongDataBin);

            if(!isCompiled)
            {
                // todo handle error with compilation
                throw new Exception();
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

            if(_globalSettings.Verbose == true)
            {
                // todo add message for the completion of this step
            }

            // no need to handle the bank defines portion
        }

        public void AssembleFinalPatch(Rom rom, List<Song> songs)
        {

            var patchData = File.ReadAllText(FileNames.AsmFiles.PatchAsm);
            var replacePatchData = Helpers.SetHexValueAfterText(patchData, ExtractedAsmDataNames.PatchAsmLocationNames.ExpARAMRetText, $"{_globalSettings.ProgramReuploadPosition:X4}");
            replacePatchData = Helpers.SetHexValueAfterText(replacePatchData, ExtractedAsmDataNames.PatchAsmLocationNames.DefARAMRetText, $"{_globalSettings.MainLoopPosition:X4}");
            replacePatchData = Helpers.SetHexValueAfterText(replacePatchData, ExtractedAsmDataNames.PatchAsmLocationNames.SongCountText, $"{songs.Count:X2}");

            var musicPointersLocation = replacePatchData.IndexOf(ExtractedAsmDataNames.PatchAsmLocationNames.MusicPointersText);
            if(musicPointersLocation == -1 )
            {
                // todo handle case where the ExtractedAsmDataNames.PatchAsmLocationNames.MusicPointersText is missing
                throw new Exception();
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
            var usedSamples = new List<AddmusicSample>();
            foreach ( var song in songs )
            {
                if(song.SongData.SongScope == SongScope.Local)
                {
                    var fileName = FileNames.BinFiles.FinalMusicDataBin($"music{song.Configuration.IntNumber:X2}{FileNames.FileExtensions.BinPatchData}");
                    var songDataFileSize = (int)(new FileInfo(fileName).Length);
                    var freespace = FindFreeSpaceInROM(rom, songDataFileSize, _globalSettings.BankStart);

                    if(freespace == -1)
                    {
                        // todo handle exception
                        throw new Exception();
                    }

                    var snesFreespace = PCToSNES(freespace);

                    musicPointerBuilder.Append($"music{song.Configuration.IntNumber:X2}+8");
                    musicIncBinsPointerBuilder.Append(PatchBuilders.MusicIncBinBuilder(freespace, song.Configuration.IntNumber));
                }
                else
                {
                    musicPointerBuilder.Append($"${0:X6}");
                }

                if((songCount % 16) == 0 && songCount != songs.Count - 1)
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
                sampleData.Add((byte)((sample.SampleDataSize + 1) & 0xFF));
                sampleData.Add((byte)((sample.SampleDataSize + 1) >> 8));
                sampleData.Add((byte)(~(sample.SampleDataSize + 1) & 0xFF));
                sampleData.Add((byte)(~(sample.SampleDataSize + 1) >> 8));
                sampleData.Add((byte)(sample.SampleDataSize & 0xFF));
                sampleData.Add((byte)(sample.SampleDataSize >> 8));

                var sampleDataStream = _fileCachingService.GetFromCache(sample.Name);
                sampleData.AddRange(sampleData.ToArray());
                var sampleFileName = FileNames.BinFiles.FinalSampleBrrDataBin($"brr{sampleCount}{FileNames.FileExtensions.BinPatchData}");
                File.WriteAllBytes(sampleFileName, sampleData.ToArray());
                var brrBinFileSize = (int)(new FileInfo(sampleFileName).Length);

                var freespace = FindFreeSpaceInROM(rom, brrBinFileSize, _globalSettings.BankStart);

                if (freespace == -1)
                {
                    // todo handle exception
                    throw new Exception();
                }
                var snesFreespace = PCToSNES(freespace);

                samplePointerBuilder.Append($"brr{sampleCount:X2}+8");
                sampleIncBinsPointerBuilder.Append(PatchBuilders.SampleBrrIncBinBuilder(freespace, sampleCount));


                sampleLoopPointerBuilder.Append($"${sample.LoopPoint:X4}");

                if(sampleCount % 16 == 0 && sampleCount != usedSamples.Count - 1)
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

            var finalSubPatch = Helpers.SetHexValueAfterText(
                subPatchBuilder.ToString(),
                ExtractedAsmDataNames.PatchAsmLocationNames.GlobalMusicCountText,
                $"{_globalSettings.ResourceList.Songs.GlobalSongs.Max(s => s.IntNumber):X2}"
            );

            if(File.Exists(FileNames.SfcFiles.TempPatchSfc))
            {
                File.Delete(FileNames.SfcFiles.TempPatchSfc);
            }

            var amUndo = File.ReadAllText(FileNames.AsmFiles.AMUndoAsm);

            File.WriteAllText(FileNames.AsmFiles.TempFinalPatch, amUndo + finalSubPatch.ToString());

            if(_globalSettings.Verbose)
            {
                // todo handle verbosity
            }

            if(_globalSettings.GeneratePatches == false)
            {
                var isPatched = PatchAsmToRom(FileNames.AsmFiles.TempFinalPatch, FileNames.SfcFiles.TempPatchSfc);

                if(!isPatched)
                {
                    // todo fix exception for failed asar patch
                    throw new Exception();
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

        public void AssembleSNESDriver()
        {

        }

        public void GenerateMSC(string romName, List<Song> songs)
        {
            var mscName = romName[..romName.LastIndexOf(".")];
            var mscBuilder = new StringBuilder();
            foreach(var song in songs)
            {
                mscBuilder.Append($"{song.Configuration.IntNumber:X2}\t0\t{song.Configuration.Name}\n");
                mscBuilder.Append($"{song.Configuration.IntNumber:X2}\t1\t{song.Configuration.Name}\n");
            }
            File.WriteAllText(mscName, mscBuilder.ToString());
        }


    }

}
