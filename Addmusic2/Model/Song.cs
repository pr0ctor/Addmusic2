using Addmusic2.Logic;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Song
    {
        public ISongNode RootNode { get; set; }
        public ISongParser Parser { get; set; }
        public string SongText { get; set; }

        public Song()
        {
            Parser = new SongParser();
        }
        public Song(ISongNode rootNode)
        {
            RootNode = rootNode;
            Parser = new SongParser();
        }

        public void ParseSong()
        {
            if (RootNode == null)
            {
                throw new Exception();
            }

            var rootNode = RootNode as SongNode;

            if(rootNode.NodeType != SongNodeType.Root)
            {
                throw new Exception();
            }

            Parser.ParseSongNodes(rootNode.Children);
        }
    }
}
