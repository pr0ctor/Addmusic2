using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.Constants;

namespace Addmusic2.Model
{
    internal class Music
    {
        public double IntroSeconds { get; set; }
        public double MainSeconds { get; set; }
        public int NoteParameterByteCount { get; set; }
        public int TempoRatio { get; set; }
        public bool NextHexIsArpeggioNoteLength { get; set; }

        public string Name { get; set; }
        public string PathlessSongName { get; set; }
        public byte[] Data { get; set; } = new byte[MagicNumbers.ChannelCount];
        public ushort[] LoopLocations { get; set; } = new ushort[MagicNumbers.ChannelCount];
        public bool PlayOnce { get; set; }
        public bool HasIntro { get; set; }
        public ushort[,] PhrasePointers = new ushort[8,2];
        public ushort[] LoopPointers = new ushort[0x10000];
        //public ushort[] LoopLengths = new ushort[0x10000];
        public string Text { get; set; }
        public int TotalSize { get; set; }
        public int SpaceForPointersAndIntegers { get; set; }

        public List<byte[]> AllPointersAndIntegers { get; set; } = new List<byte[]>();
        public List<byte[]> InstrumentData { get; set; } = new List<byte[]>();
        public List<byte[]> FinalData { get; set; } = new List<byte[]>();

        public SpaceInfo SpaceInfo { get; set; }

        public uint IntroLength { get; set; }
        public uint MainLength { get; set; }
        public uint Seconds { get; set; }

        public bool HasYoshiDrums { get; set; }
        public bool KnowsLength { get; set; }
        public int Index { get; set; }

        public List<ushort> Samples { get; set; } = new List<ushort>();
        public int EchoBufferSize { get; set; }
        public bool HasEchoBufferCommend { get; set; }
        public bool EchoBufferAlloVCMDIsSet { get; set; }
        public ushort EchoBufferAllocVCMDILocation { get; set; }
        public int EchoBufferAllocVCMDIChanner { get; set; }

        public string StatString { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Game { get; set; }
        public string Comment { get; set; }

        public bool[] UsedSamples { get; set; } = new bool[MagicNumbers.MaxSamplesCount];

        public int MinSize { get; set; }
        public bool Exists { get; set; }
        public int PositionInARAM { get; set; }
        public int RemoteDefinitionType { get; set; }
        public bool InRemoteDefinition { get; set; }
        //public int RemoteDefinitionArg { get; set; }

        public Dictionary<string, string> Replacements { get; set; } = new Dictionary<string, string>();

        private bool guessLength;
        private int resizedChannel;
        private double[] channelLengths { get; set; } = new double[8];               // How many ticks are in each channel.
        private double[] loopLengths { get; set; } = new double[0x10000];                // How many ticks are in each loop.
        private double normalLoopLength;                // How many ticks were in the most previously declared normal loop.
        private double superLoopLength;                 // How many ticks were in the most previously declared super loop.
        //private std::vector<std::pair<double, int>> tempoChanges;   // Where any changes in tempo occur. A negative tempo marks the beginning of the main loop, if an intro exists.
        private bool baseLoopIsNormal;
        private bool baseLoopIsSuper;
        private bool extraLoopIsNormal;
        private bool extraLoopIsSuper;

        public Music()
        {

        }

        public void Init()
        {

        }
        public bool DoReplacement()
        {

        }
    }
}
