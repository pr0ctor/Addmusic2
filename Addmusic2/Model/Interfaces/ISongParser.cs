using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface ISongParser
    {

        public SongData ParseSongNodes(List<ISongNode> nodes);
        public IValidationResult ValidateNode(ISongNode node);
    }
}
