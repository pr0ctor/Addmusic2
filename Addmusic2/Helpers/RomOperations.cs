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

        public RomOperations(IGlobalSettings settings, ILogger<IAddmusicLogic> logger, MessageService messageService, IFileCachingService fileCachingService)
        {
            _globalSettings = (GlobalSettings)settings;
            _messageService = messageService;
            _logger = logger;
            _fileCachingService = (FileCachingService)fileCachingService;
        }

        public int SNESToPC(int address)
        {
            if (address < 0 || address > 0xFFFFFF ||     // not 24bit
                (address & 0xFE0000) == 0x7E0000 ||     // wram
                (address & 0x408000) == 0x000000)     // hardward registers
            {
                return -1;
            }

            if (_globalSettings.EnableSA1Addressing && address >= 0x808000)
            {
                address -= 0x400000;
            }

            address = ((address & 0x7F0000) >> 1 | (address & 0x7FFF));

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

            var sfx1DF9Max = sfx1DF9.MaxBy(s => s.Configuration.IntNumber).Configuration.IntNumber;
            var missing1DF9 = Enumerable.Range(0, sfx1DF9Max).Except(sfx1DF9.Select(s => s.Configuration.IntNumber));

            var sfx1DFCMax = sfx1DFC.MaxBy(s => s.Configuration.IntNumber).Configuration.IntNumber;
            var missing1DFC = Enumerable.Range(0, sfx1DF9Max).Except(sfx1DFC.Select(s => s.Configuration.IntNumber));

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

        public void AssembleSNESDriver()
        {

        }

        public void GenerateMSC(string romName, List<string> songNames)
        {

        }


    }


    //internal class RomOperations : IRomOperations
    //{
    //    private GlobalSettings Settings { get; set; }

    //    public RomOperations(IGlobalSettings settings)
    //    {
    //        Settings = (GlobalSettings)settings;
    //    }

    //    public int SNESToPC(int address)
    //    {
    //        if(address < 0 || address > 0xFFFFFF ||     // not 24bit
    //            (address & 0xFE0000) == 0x7E0000 ||     // wram
    //            (address & 0x408000) == 0x000000)     // hardward registers
    //        {
    //            return -1;
    //        }

    //        if(Settings.EnableSA1Addressing && address >= 0x808000)
    //        {
    //            address -= 0x400000;
    //        }

    //        address = ((address & 0x7F0000) >> 1 | (address & 0x7FFF));

    //        return address;
    //    }

    //    public int PCToSNES(int address)
    //    {
    //        if(address < 0 || address >= 0x400000)
    //        {
    //            return -1;
    //        }

    //        address = ((address << 1) & 0x7F0000) | (address & 0x7FFF) | 0x8000;

    //        if(!Settings.EnableSA1Addressing && (address & 0xF00000) == 0x700000)
    //        {
    //            address |= 0x800000;
    //        }

    //        if(Settings.EnableSA1Addressing && address >= 0x400000)
    //        {
    //            address += 0x400000;
    //        }

    //        return address;
    //    }

    //    public bool FindRATS(byte[] romData, int offset)
    //    {
    //        if (romData[offset] != 0x53)
    //        {
    //            return false;
    //        }
    //        if (romData[offset] != 0x54)
    //        {
    //            return false;
    //        }
    //        if (romData[offset] != 0x41)
    //        {
    //            return false;
    //        }
    //        if (romData[offset] != 0x52)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public int ClearRATS(byte[] romData, int offset)
    //    {
    //        int size = ((romData[offset + 5] << 8) | romData[offset + 4]) + 8;
    //        int r = size;
    //        while (size >= 0)
    //        {
    //            romData[offset + size--] = 0;
    //        }
    //        return r + 1;
    //    }
    //}
}
