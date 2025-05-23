using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal class SongNode : ISongNode
    {
        public SongNodeType NodeType { get; set; }

        public string NodeSource { get; set; }

        public ISongNodePayload Payload { get; set; }

        public List<ISongNode> Children { get; set; }

        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public SongNode()
        {

        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var child in Children)
            {
                builder.Append(child.ToString());
            }

            return builder.ToString();
        }
    }
}
