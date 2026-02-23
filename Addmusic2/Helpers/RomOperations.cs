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
        private IAddmusicLogger _logger;
        private GlobalSettings _globalSettings;
        private MessageService _messageService;
        private IFileCachingService _fileCachingService;

        public RomOperations(IGlobalSettings settings, IAddmusicLogger logger, MessageService messageService, IFileCachingService fileCachingService)
        {
            _globalSettings = (GlobalSettings)settings;
            _messageService = messageService;
            _logger = logger;
            _fileCachingService = fileCachingService;

        }

        public void ClearRATSTag(ref Rom rom, int offset)
        {
            _logger.LogInformation(LogLevel.Debug, $"Clearing RATS tag from Rom({rom.RomFileName}) at offset({offset})");
            var size = ((rom.RomData[offset + 5] << 8) | rom.RomData[offset + 4]) + 8;
            rom.RomData.RemoveRange(offset, size);
            rom.RomData.InsertRange(offset, Enumerable.Repeat<byte>(0, size));
        }

        public Rom LoadRomData()
        {
            var romPath = "";

            // catches full filepath or name is in the current location that the program can pull from
            if (File.Exists(_globalSettings.RomName))
            {
                _logger.LogInformation(LogLevel.Debug, $"Found ROM file [{_globalSettings.RomName}]");
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
                    _logger.LogError(LogLevel.Critical, _messageService.GetErrorCannotFindRomInLocationsMessage(_globalSettings.RomName), true);
                    throw new FileNotFoundException(_messageService.GetErrorCannotFindRomInLocationsMessage(_globalSettings.RomName));
                }
                _logger.LogInformation(LogLevel.Debug, $"Found ROM File at location: {{{romPath}}}");
            }

            // Fix cases of mixed slashes and standardize them for the current operation system file structure
            var standardizedPath = Helpers.StandardizeFileDirectoryDelimiters(romPath);
            var lastDirectorySeparatorIndex = Helpers.GetLastDirectorySeparatorIndex(standardizedPath);
            var romName = standardizedPath[lastDirectorySeparatorIndex..];
            var romInfo = new FileInfo(standardizedPath);
            var rom = new Rom(_messageService, this, _logger)
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
            var rom = new Rom(_messageService, this, _logger)
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

        public int FindFreeSpaceInROM(Rom rom, int size, int start)
        {
            _logger.LogInformation(LogLevel.Debug, $"Searching {rom.RomFileName} for freespace of length {size} starting at index {start}");
            if(rom.RomData.Count == 0)
            {
                throw new Exception("Rom Data not loaded.");
            }

            // Handle case where the size of the freespace to find is 0
            if(size == 0)
            {
                _logger.LogError(LogLevel.Warning, $"Cannot find freespace where the given length of the freespace is 0.", true);
                throw new ArgumentException($"Cannot find freespace where the given length of the freespace is 0.");
            }

            // Handle case where the freespace to search for is larger than 4KiB
            if(size > MagicNumbers.FourKiBRomSize)
            {
				_logger.LogError(LogLevel.Warning, $"Cannot find freespace in Rom({rom.RomFileName}) where the specified size({size}) is larger than {MagicNumbers.FourKiBRomSize} .", true);
				throw new ArgumentException($"Cannot find freespace in Rom({rom.RomFileName}) where the specified size({size}) is larger than {MagicNumbers.FourKiBRomSize} .");
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

                    // If there's a size mismatch or if theres technically a sequence match but its not a RATS tag
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
				_logger.LogError(LogLevel.Critical, $"Failed to find freespace in Rom({rom.RomFileName}) of size({size}) starting at index({start})", true);
				if (start == 0x080000)
                {
                    return -1;
                }
                else
                {
                    return FindFreeSpaceInROM(rom, size, 0x080000);
                }
            }

            var insertPosition = position - size;

			_logger.LogInformation(LogLevel.Debug, $"Found freespace at {insertPosition}.");

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

        public bool CompileAsmToBin(string sourceFileName, string binToWrite, string tempFileName = "")
        {
            _logger.LogInformation(LogLevel.Debug, $"Compiling Asm({sourceFileName}) to .bin({binToWrite})");
            var tempFile = (tempFileName.Length > 0) ? tempFileName : FileNames.StaticFiles.TempTextFile;
            using var tempTextFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, tempFile), false);
            using var tempLogFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempLogFile), true);

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

            if(notifications.Length > 0)
            {
                foreach( var notification in notifications )
                {
                    tempTextFileWriter.WriteLine( notification );
                }
            }

			var messageBuilder = LogAsarMessages(notifications, warnings, errors);

			if (warnings.Length > 0 || errors.Length > 0)
            {
                tempLogFileWriter.WriteLine(messageBuilder.ToString());
                return false;
            }
            //using var binFile = File.Open(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempBinFile), FileMode.OpenOrCreate);
            using var binFile = File.Open(binToWrite, FileMode.OpenOrCreate);
            binFile.Write(dataOutArray);
            binFile.Flush();
            binFile.Close();
            return true;
        }

        public bool PatchAsmToRom(string sourceFileName, string romToPatch)
        {
			_logger.LogInformation(LogLevel.Debug, $"Compiling Asm({sourceFileName}) to Rom({romToPatch})");
			using var tempTextFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempTextFile), true);
            using var tempLogFileWriter = new StreamWriter(Path.Combine(FileNames.ExecutionLocations.InstallLocation, FileNames.FolderNames.LogFolder, FileNames.StaticFiles.TempLogFile), true);

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

            var messageBuilder = LogAsarMessages(notifications, warnings, errors);

			if (warnings.Length > 0 || errors.Length > 0)
			{
				tempLogFileWriter.WriteLine(messageBuilder.ToString());
				return false;
			}

			using var sfcFile = File.Open(FileNames.SfcFiles.TempPatchSfc, FileMode.OpenOrCreate);
            sfcFile.Write(romBytes);
            sfcFile.Flush();
            sfcFile.Close();
            return true;
        }

        public StringBuilder LogAsarMessages(string[]? notifications, Asarerror[]? warnings, Asarerror[]? errors)
        {
			var notificationMessages = notifications?.ToList() ?? new List<string>();
			var warningMessages = warnings?.Select(w => w.Fullerrdata).ToList() ?? new List<string>();
			var errorMessages = errors?.Select(w => w.Fullerrdata).ToList() ?? new List<string>();

			var fullStringBuilder = new StringBuilder();
            var notificationBuilder = new StringBuilder();
            var warningBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            if(notificationMessages.Count > 0)
            {
                notificationBuilder.AppendLine("Asar Notifications: ");
                foreach( var notification in notificationMessages)
                {
                    notificationBuilder.AppendLine(notification);
                }
                _logger.LogInformation(LogLevel.Debug, notificationBuilder.ToString());
                fullStringBuilder.AppendLine(notificationBuilder.ToString());
            }

			if (warningMessages.Count > 0)
			{
				warningBuilder.AppendLine("Asar Warnings: ");
				foreach (var warning in warningMessages)
				{
					warningBuilder.AppendLine(warning);
				}
                _logger.LogWarning(LogLevel.Warning, warningBuilder.ToString());
                fullStringBuilder.AppendLine(warningBuilder.ToString());
			}

			if (errorMessages.Count > 0)
			{
				errorBuilder.AppendLine("Asar Errors: ");
				foreach (var error in errorMessages)
				{
					errorBuilder.AppendLine(error);
				}
				_logger.LogError(LogLevel.Critical, errorBuilder.ToString());
				fullStringBuilder.AppendLine(errorBuilder.ToString());
			}

            return fullStringBuilder;
		}


    }

}
