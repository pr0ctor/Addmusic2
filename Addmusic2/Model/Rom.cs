using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
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
        public string RomFileName { get; set; } = string.Empty;
        public string RomFilePath { get; set; } = string.Empty;
        public string RomFileExtension {  get; set; } = string.Empty;
        public long RomFileSize { get; set; } = 0;
        public bool IsRomSA1 { get; set; } = false;
        public bool AllowSA1 { get; set; } = false;
        public List<byte> RomHeader { get; set; } = new();
        public List<byte> RomData { get; set; } = new();

        public Rom(MessageService messageService, IRomOperations romOperations)
        {
            _messageService = messageService;
            _romOperations = romOperations;
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
        }

        public Rom CreateTempRomCopy()
        {
            var newTempRom = new Rom(_messageService, _romOperations)
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
            var newTempRom = new Rom(_messageService, _romOperations)
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
            var newTempRom = new Rom(_messageService, _romOperations)
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
            if (!fileName.Contains(FileNames.FileExtensions.RomSmc) && !fileName.Contains(FileNames.FileExtensions.RomSfc))
            {
                // todo fix exception message
                throw new ArgumentException();
            }

            if (!File.Exists(path))
            {
                // todo fix exception message
                throw new FileNotFoundException();
            }

            var romData = File.ReadAllBytes(path).ToList();

            if(romData.Count <= MagicNumbers.RomMinimumSize)
            {
                // todo fix exception message
                throw new Exception();
            }

            // validate that the rom is of an expected size
            if(romData.Count % MagicNumbers.RomSizeMultiple == 0)
            {
                // Get the Header Bytes of the Rom
                RomHeader = romData.GetRange(0, MagicNumbers.RomHeaderLength);
                // Get the rest of the bytes that aren't the header
                RomData = romData.GetRange(MagicNumbers.RomHeaderLength + 1, romData.Count - MagicNumbers.RomHeaderLength);
            }

            if(_romOperations.SNESToPC(MagicNumbers.SA1CheckBitLocation) == MagicNumbers.SA1CheckBitValue && AllowSA1 == true)
            {
                IsRomSA1 = true;
            }
            
        }

    }
}
