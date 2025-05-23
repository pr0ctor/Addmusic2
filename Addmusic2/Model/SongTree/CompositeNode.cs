using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal class CompositeNode : SongNode
    {
        public CompositeNode() { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
