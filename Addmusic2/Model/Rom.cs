using Addmusic2.Exceptions;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Rom : IRomData
    {
        private MessageService _messageService;
        private IRomOperations _romOperations;
        private IAddmusicLogger _logger;
        public string RomFileName { get; set; } = string.Empty;
        public string RomFilePath { get; set; } = string.Empty;
        public string RomFileExtension {  get; set; } = string.Empty;
        public long RomFileSize { get; set; } = 0;
        public bool IsRomSA1 { get; set; } = false;
        public bool AllowSA1 { get; set; } = false;
        public List<byte> RomHeader { get; set; } = new();
        public List<byte> RomData { get; set; } = new();

        public Rom(MessageService messageService, IRomOperations romOperations, IAddmusicLogger logger)
        {
            _messageService = messageService;
            _romOperations = romOperations;
            _logger = logger;
        }

        /*public Rom(MessageService messageService, string fileName, string filePathWithFileName)
        {
            _messageService = messageService;
            RomFileName = fileName;
            RomFilePath = filePathWithFileName;
        }*/

        public void WriteRomDataToFile()
        {
            WriteRomData(RomFilePath);
        }

        public void WriteRomDataToFile(string filePath)
        {
            WriteRomData(filePath);
        }

        protected void WriteRomData(string filePath)
        {
            // rebuild the full rom file by combining the header and the data
            var fullList = new List<byte>();
            fullList.AddRange(RomHeader);
            fullList.AddRange(RomData);
            File.WriteAllBytes(filePath, fullList.ToArray());
            _logger.LogInformation(LogLevel.Debug, $"Wrote {fullList.Count} bytes to {filePath}");
        }

        public Rom CreateTempRomCopy()
        {
            var newTempRom = new Rom(_messageService, _romOperations, _logger)
            {
                RomFileName = RomFileName,
                RomFilePath = RomFilePath,
                RomFileExtension = RomFileExtension,
                RomFileSize = RomFileSize,
                IsRomSA1 = IsRomSA1,
                AllowSA1 = AllowSA1,
                RomHeader = [.. RomHeader],
                RomData = [.. RomData]
            };

            return newTempRom;
        }

        public Rom CreateTempRomCopy(string romFileName)
        {
            var newTempRom = new Rom(_messageService, _romOperations, _logger)
            {
                RomFileName = romFileName,
                RomFilePath = RomFilePath.Replace(RomFileName, romFileName),
                RomFileExtension = RomFileExtension,
                RomFileSize = RomFileSize,
                IsRomSA1 = IsRomSA1,
                AllowSA1 = AllowSA1,
                RomHeader = [.. RomHeader],
                RomData = [.. RomData]
            };

            return newTempRom;
        }

        public Rom CreateTempRomCopy(string romFileName, string fileExtension)
        {
            var newTempRom = new Rom(_messageService, _romOperations, _logger)
            {
                RomFileName = romFileName,
                RomFilePath = RomFilePath.Replace(RomFileName, romFileName).Replace(RomFileExtension, fileExtension),
                RomFileExtension = fileExtension,
                RomFileSize = RomFileSize,
                IsRomSA1 = IsRomSA1,
                AllowSA1 = AllowSA1,
                RomHeader = [.. RomHeader],
                RomData = [.. RomData]
            };

            return newTempRom;
        }

        public void LoadRomData()
        {
            LoadRomDataFromFile(RomFilePath, RomFileName);
        }

        public void LoadRomFile(string path, string fileName)
        {
            LoadRomDataFromFile(path, fileName);
        }

        protected void LoadRomDataFromFile(string path, string fileName)
        {
            _logger.LogInformation(LogLevel.Debug, $"Loading ROM({fileName}) at path({path})");
            if (!fileName.Contains(FileNames.FileExtensions.RomSmc) && !fileName.Contains(FileNames.FileExtensions.RomSfc))
            {
                _logger.LogError(LogLevel.Debug, _messageService.GetErrorInvalidRomFileExtensionMessage(fileName, $" {FileNames.FileExtensions.RomSmc}, {FileNames.FileExtensions.RomSfc}"));
                throw new ArgumentException(_messageService.GetErrorInvalidRomFileExtensionMessage(fileName, $" {FileNames.FileExtensions.RomSmc}, {FileNames.FileExtensions.RomSfc}"));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(_messageService.GetErrorRomFileNotFoundMessage(fileName, path));
            }

            // read the Rom data in as a list
            var romData = File.ReadAllBytes(path).ToList();

            if(romData.Count <= MagicNumbers.RomMinimumSize)
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorRomLessThanMinimumSizeMessage(fileName, romData.Count.ToString(), MagicNumbers.RomMinimumSize.ToString()), true);
                throw new InvalidConfigurationException(_messageService.GetErrorRomLessThanMinimumSizeMessage(fileName, romData.Count.ToString(), MagicNumbers.RomMinimumSize.ToString()));
            }

            // validate that the rom is of an expected size
            if(romData.Count % MagicNumbers.RomSizeMultiple != 0)
            {
                // Get the Header Bytes of the Rom
                RomHeader = romData.GetRange(0, MagicNumbers.RomHeaderLength);
                // Get the rest of the bytes that aren't the header
                RomData = romData.GetRange(MagicNumbers.RomHeaderLength, romData.Count - MagicNumbers.RomHeaderLength);
            }
            else
            {
                _logger.LogError(LogLevel.Error, _messageService.GetErrorRomUnexpectedFileSizeMessage(fileName), true);
                throw new InvalidConfigurationException(_messageService.GetErrorRomUnexpectedFileSizeMessage(fileName));
            }

            // validate that the Rom has a valid check bit for SA1 and note if that is the case assuming SA1 is allowed
            if (_romOperations.SNESToPC(MagicNumbers.SA1CheckBitLocation) == MagicNumbers.SA1CheckBitValue && AllowSA1 == true)
            {
                IsRomSA1 = true;
            }
            _logger.LogInformation(LogLevel.Debug, $"Loaded {romData.Count} bytes for ROM({fileName}) at path({path})");
        }

    }
}
