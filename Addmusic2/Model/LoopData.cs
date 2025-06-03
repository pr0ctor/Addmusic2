using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class LoopData
    {
        public ushort LoopId { get; set; }
        public LoopNode LoopNode { get; set; } 
        public ushort LoopLengthInTicks { get; set; }
    }
}
