using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class ChannelInformation
    {

        public int ChannelNumber { get; set; }
        public List<byte> ChannelData { get; set; } = new();
        public List<byte> LoopLocations { get; set; } = new();
        public double ChannelLength { get; set; }
        public bool HasIntro { get; set; } = false;
        public byte PhraseLocation { get; set; } = 0;
        public byte IntroLocation { get; set; } = 0;
        public int IntroLength { get; set; } = 0;
        public bool HasNoteData { get; set; } = false;
        public byte CurrentQuantization { get; set; }
        public bool UpdateQuantization { get; set; } = false;
        public int CurrentInstrument { get; set; }
        public bool IgnoreTuning { get; set; } = false;

        public ChannelInformation() { }
    }
}
