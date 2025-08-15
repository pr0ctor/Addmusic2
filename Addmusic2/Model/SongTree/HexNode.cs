using Addmusic2.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal class HexNode : SongNode
    {
        public HexCommands CommandType { get; set; }
        public string HexCommand { get; set; }
        public List<string> HexValues { get; set; } = new();
        public HexNode() { }
    }
}
