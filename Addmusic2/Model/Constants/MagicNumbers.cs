using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class MagicNumbers
    {
        public static readonly int ChannelCount = 9;
        public static readonly int MaxSamplesCount = 256;
        public static readonly int[] ValidPitches = { 9, 11, 0, 2, 4, 5, 7 };
        public static readonly int[] TempTrans = { 0, 0, 5, 0, 0, 0, 0, 0, 0, -5, 6, 0, -5, 0, 0, 8, 0, 0, 0 };
        public static readonly int[] InstrumentsToSample = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x07, 0x08, 0x09, 0x05, 0x0A,	// \ Instruments
            0x0B, 0x01, 0x10, 0x0C, 0x0D, 0x12, 0x0C, 0x11, 0x01,		// /
            0x00, 0x00,							                        // Nothing
            0x0F, 0x06, 0x06, 0x0E, 0x0E, 0x0B, 0x0B, 0x0B, 0x0E        // Percussion
        };
        public static readonly int[] HexLengths = { 
            2, 2, 3, 4, 4, 1, 2, 3, 2, 3, 2, 4, 2, 2, 3, 4, 2, 4, 4, 3, 2, 4,
            1, 4, 4, 3, 2, 9, 3, 4, 2, 3, 3, 2, 5, 1, 1 
        };
    }
    
}
