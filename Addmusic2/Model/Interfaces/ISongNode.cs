using Addmusic2.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface ISongNode
    {
        public SongNodeType NodeType { get; set; }
        public ISongNodePayload Payload { get; set; }
        public List<ISongNode> Children { get; set; }

        string ToString();
    }
}
