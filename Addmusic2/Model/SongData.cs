using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.Constants;

namespace Addmusic2.Model
{
    internal class SongData
    {
        public Guid SongId { get; set; }
        public string SongPath { get; set; } = string.Empty;
        public SongScope SongScope { get; set; }
        public SampleInstrumentManager SampleInstrumentManager { get; set; } = new();
        public AddmusicKVersion AmkVersion { get; set; } = AddmusicKVersion.Version2;
        public AddmusicKParserVersion AmkParserVersion { get; set; } = AddmusicKParserVersion.Version0;

        public List<byte> RatsData { get; set; } = new();
        public List<byte> FinalData { get; set; } = new();
        
        public List<(double ChannelTick, int TempoChange)> TempoChanges { get; set; } = new();
        public int[] TransposeMap = [.. MagicNumbers.TempTrans,.. new int[256 - MagicNumbers.TempTrans.Length]];
        public int Seconds { get; set; }
        public int IntroSeconds { get; set; }
        public int MainSeconds { get; set; }
        public int NoteParameterByteCount { get; set; }
        public int TempoRatio { get; set; }
        public bool NextHexIsArpeggioNoteLength { get; set; }

        public string Name { get; set; } = string.Empty;
        public string PathlessSongName { get; set; } = string.Empty;
        public List<ChannelInformation> ChannelData { get; set; } = new();
        public bool[,] NoMusic { get; set; } = new bool[8,2];
        public bool HasIntro { get; set; }
        public ushort[,] PhrasePointers = new ushort[8,2];
        public ushort[] LoopPointers = new ushort[0x10000];
        public string Text { get; set; } = string.Empty;
        public int TotalSize { get; set; }
        public int SpaceForPointersAndInstruments { get; set; }
        public int SpaceUsedBySamples { get; set; }

        public List<byte> AllPointersAndInstruments { get; set; } = new();

        public SongSpaceInformation SpaceInfo { get; set; } = new();

        public int IntroLength { get; set; }
        public int MainLength { get; set; }
        

        public bool HasYoshiDrums { get; set; }
        public bool KnowsLength { get; set; }
        public int EchoBufferSize { get; set; }
        public bool HasEchoBufferCommand { get; set; }
        public bool EchoBufferAllocVCMDIsSet { get; set; }
        public ushort EchoBufferAllocVCMDILocation { get; set; }
        public int EchoBufferAllocVCMDIChannel { get; set; }

        public string StatString { get; set; } = string.Empty;
        public string Title { get; set; } = MagicNumbers.StringValues.DefaultSpcTitleName;
        public string Author { get; set; } = string.Empty;
        public string Game { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public int MinSize { get; set; }
        public int PositionInARAM { get; set; }

        public bool GuessLength { get; set; }
        public bool DoesntLoop { get; set; }

        public VelocityTable VelocityTable { get; set; }

        public SongData()
        {
            SongId = Guid.NewGuid();
        }
    }
}
