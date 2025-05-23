using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal class LoopNode : SongNode
    {

        public List<ISongNode> LoopContents { get; set; } = new List<ISongNode>();
        public string LoopName { get; set; }
        public int Iterations { get; set; }


    }
}
