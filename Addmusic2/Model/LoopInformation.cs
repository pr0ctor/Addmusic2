using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class LoopInformation
    {
        public ushort LoopId { get; set; }
        public LoopNode LoopNode { get; set; } 
        public ushort LoopLengthInTicks { get; set; }
        public byte CurrentQuantization { get; set; } = MagicNumbers.DefaultValues.InitialChannelQuantizationValue;
        public bool UpdateQuantization { get; set; } = true;
        public int CurrentInstrument { get; set; }
        public int LoopLocation { get; set; }
#pragma warning disable 8618
        public LoopInformation() { }
#pragma warning restore 8618
    }
}
