using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal class AddmusicKCheckBits : IAddmusicCheckBits
    {
        public readonly int CleanRomFirstCheckBitLocation = 0x70000;
        public readonly int CleanRomFirstCheckBitValue = 0x3E;
        public readonly int CleanRomSecondCheckBitLocation = 0x70000;
        public readonly int CleanRomSecondCheckBitValue = 0x0E;
        public readonly int AmkCheckValueLocation = 0x0E8000;
        public readonly int AmkCheckValueLength = 4;
        public readonly string AmkCheckValueStringValue = "@AMK";
        public readonly int AmkDataVersionLocation = 0x0E8004;
    }
}
