using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model.Interfaces;

namespace Addmusic2.Model.SongTree
{
    internal class SongTree
    {

        private List<ISongNode> Nodes;

        public SongTree()
        {
            Nodes = new List<ISongNode>();
        }

        public SongTree(List<ISongNode> nodes)
        {
            Nodes = nodes;
        }

        public void AddNode(ISongNode node)
        {
            Nodes.Add(node);
        }

        public List<ISongNode> GetNodes()
        {
            return Nodes;
        }

    }
}
