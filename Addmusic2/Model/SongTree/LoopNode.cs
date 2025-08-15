using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal class LoopNode : SongNode, ICloneable
    {

        public List<ISongNode> LoopContents { get; set; } = new List<ISongNode>();
        public string LoopName { get; set; }
        public int Iterations { get; set; }

        public object Clone()
        {
            return new LoopNode
            {
                NodeSource = this.NodeSource,
                NodeType = this.NodeType,
                Payload = this.Payload,
                LineNumber = this.LineNumber,
                ColumnNumber = this.ColumnNumber,
                LoopContents = this.LoopContents,
                Iterations = this.Iterations,
                LoopName = this.LoopName,
            };
        }
    }
}
