using Addmusic2.Model.Constants;
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
        //public List<byte> LoopLocations { get; set; } = new();
        public List<ushort> LoopLocations { get; set; } = new();
        public double ChannelLength { get; set; }
        public bool HasIntro { get; set; } = false;
        public byte PhraseLocation { get; set; } = 0;
        public byte IntroLocation { get; set; } = 0;
        public int IntroLength { get; set; } = 0;
        public bool HasNoteData { get; set; } = false;
        public byte CurrentQuantization { get; set; } = MagicNumbers.DefaultValues.InitialChannelQuantizationValue;
        public bool UpdateQuantization { get; set; } = true;
        public int CurrentInstrument { get; set; }
        public bool IgnoreTuning { get; set; } = false;
        public bool NoMusic { get; set; } = false;

        public ChannelInformation() { }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('#');
            builder.Append(ChannelNumber);
            builder.Append(" - size = ");
            builder.Append(ChannelData.Count);
            return builder.ToString();
        }
    }
}
