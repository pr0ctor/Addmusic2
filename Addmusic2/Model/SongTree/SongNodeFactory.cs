using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.SongTree
{
    internal static class SongNodeFactory
    {
        public static ISongNode DetermineSongNodeFromParser()
        {

            return new SongNode();
            //return new ChannelNode();
        }
    }
}
