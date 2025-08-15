using Addmusic2.Model;
using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Helpers
{
    internal class RomOperations : IRomOperations
    {
        private GlobalSettings Settings { get; set; }

        public RomOperations(IGlobalSettings settings)
        {
            Settings = (GlobalSettings)settings;
        }

        public int SNESToPC(int address)
        {
            if(address < 0 || address > 0xFFFFFF ||     // not 24bit
                (address & 0xFE0000) == 0x7E0000 ||     // wram
                (address & 0x408000) == 0x000000)     // hardward registers
            {
                return -1;
            }

            if(Settings.EnableSA1Addressing && address >= 0x808000)
            {
                address -= 0x400000;
            }

            address = ((address & 0x7F0000) >> 1 | (address & 0x7FFF));

            return address;
        }

        public int PCToSNES(int address)
        {
            if(address < 0 || address >= 0x400000)
            {
                return -1;
            }

            address = ((address << 1) & 0x7F0000) | (address & 0x7FFF) | 0x8000;

            if(!Settings.EnableSA1Addressing && (address & 0xF00000) == 0x700000)
            {
                address |= 0x800000;
            }

            if(Settings.EnableSA1Addressing && address >= 0x400000)
            {
                address += 0x400000;
            }

            return address;
        }

        public bool FindRATS(byte[] romData, int offset)
        {
            if (romData[offset] != 0x53)
            {
                return false;
            }
            if (romData[offset] != 0x54)
            {
                return false;
            }
            if (romData[offset] != 0x41)
            {
                return false;
            }
            if (romData[offset] != 0x52)
            {
                return false;
            }
            return true;
        }

        public int ClearRATS(byte[] romData, int offset)
        {
            int size = ((romData[offset + 5] << 8) | romData[offset + 4]) + 8;
            int r = size;
            while (size >= 0)
            {
                romData[offset + size--] = 0;
            }
            return r + 1;
        }
    }
}
