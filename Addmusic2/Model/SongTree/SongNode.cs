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

        public string NodeSource { get; set; } = string.Empty;

        public ISongNodePayload Payload { get; set; }

        public List<ISongNode> Children { get; set; } = new();

        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

#pragma warning disable 8618
        public SongNode() { }
#pragma warning restore 8618

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(NodeSource);

            return builder.ToString();
        }
    }
}
