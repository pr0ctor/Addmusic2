using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface IRomOperations
    {

        int FindFreeSpaceInROM(Rom rom, int size, int start);

        void ClearRATSTag(ref Rom rom, int offset);

        int SNESToPC(int address);
        int SNESToPC(int address, bool useSA1);
        int PCToSNES(int address);
        int PCToSNES(int address, bool useSA1);
    }
}
