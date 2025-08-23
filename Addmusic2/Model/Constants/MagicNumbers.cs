using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class MagicNumbers
    {

        public static class DefaultValues
        {
            public static readonly int DefaultBankStart = 0x200000;
            public static readonly int DefaultBankStartFromCLArgs = 0x080000;
            public static readonly int InitialDefaultNoteLength = NoteLengthMaximum / 8;
            public static readonly int StartingOctave = 4;
            public static readonly int InitialTempoRatio = 1;
            public static readonly int IntialSpaceForPointersAndStrumentsValue = 20;
            public static readonly int InitialTempoValue = 0x36;
            public static readonly int InitialSfxLeftVolume = 0x7F;
            public static readonly int InitialSfxRightVolume = 0x7F;
            public static readonly int InitialSfxNoteLength = 8;
        }

        public static readonly int NoteLengthMaximum = 192;
        public static readonly byte NoteLengthMaxBeforeSplit = 0x80;
        public static readonly byte NoteLengthDecreaseFactor = 0x60;
        public static readonly int EightBitMaximum = 255;
        public static readonly ushort SixteenBitMaximum = 0xFFFF;
        public static readonly int ThirtytwoBitMaximum = 0xFFFFFF;
        public static readonly byte HexCommandMaximum = 0xFF;
        public static readonly int PanDirectionMaximum = 20;
        public static readonly byte NoiseMaximum = 0x1F;
        public static readonly int SpcTextMaximumLength = 32;
        public static readonly byte ByteHexMaximum = 0xFF;
        public static readonly int OctaveMinimum = -1;
        public static readonly int OctaveMaximum = 7;
        public static readonly int StartingCustomInstrumentNumber = 30;
        public static readonly int SampleSCRNTableSize = 4;
        public static readonly int SfxVolumeMaximum = 127;
        public static readonly int SfxInstrumentMaximum = 0x7F;

        // public static readonly int SampleBankRequiredSize = 0x8000;



        public static readonly int ChannelCount = 8;
        public static readonly int MaxSamplesCount = 256;
        public static readonly int[] ValidPitches = { 9, 11, 0, 2, 4, 5, 7 };
        public static readonly byte PitchOffset = 0x61;
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

        public static List<int> SfxChannels = new List<int>
        {
            6,
            7,
        };

        public static List<byte> ChannelAdjustmentBytes = new()
        {
            0xFA,
            0x06,
            0x01,
        };

        public static Func<byte,List<byte>> EchoBufferAdjustmentBytes = (byte bufferSize) =>
        {
            return new List<byte>
            {
                0xFA,
                0x04,
                bufferSize,
            };
        };

        public static class CommandValues
        {
            public static readonly byte Tie = 0xC6;
            public static readonly byte Rest = 0xC7;
            public static readonly byte Instrument = 0xDA;
            public static readonly byte Pan = 0xDB;
            public static readonly byte PitchSlide = 0xDD;
            public static readonly byte Vibrato = 0xDE;
            public static readonly byte GlobalVolume = 0xE0;
            public static readonly byte GlobalVolumeWithFade = 0xE1;
            public static readonly byte Tempo = 0xE2;
            public static readonly byte TempoWithFade = 0xE3;
            public static readonly byte SuperLoop = 0xE6;
            public static readonly byte Volume = 0xE7;
            public static readonly byte VolumeWithFade = 0xE8;
            public static readonly byte Loop = 0xE9;
            public static readonly byte SfxPitchSlide = 0xEB;
            public static readonly byte SampleLoad = 0xF3;
            public static readonly byte Noise = 0xF8;
            public static readonly byte RemoteCode = 0xFC;
            public static readonly byte SfxJsrCommand = 0xFD;
        }
    }
    
}
