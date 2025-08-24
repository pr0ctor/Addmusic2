using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using Addmusic2.Parsers;
using Microsoft.Extensions.Logging;
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
        public string SongText { get; set; } = string.Empty;
        public SongData SongData { get; set; } = new();

        public Song(SongParser parser)
        {
            Parser = parser;
        }
        public Song(SongParser parser, ISongNode rootNode)
        {
            RootNode = rootNode;
            Parser = parser;
        }

        public void ParseSong()
        {
            if (RootNode == null)
            {
                throw new Exception();
            }

            var rootNode = RootNode as SongNode;

            if(rootNode == null || rootNode.NodeType != SongNodeType.Root)
            {
                throw new Exception();
            }

            SongData = Parser.ParseSongNodes(rootNode.Children);
        }
    }
}
