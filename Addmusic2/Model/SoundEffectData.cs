using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class SoundEffectData
    {

        public string Name { get; set; }
        public string PatchData { get; set; }

        public int AramPosition { get; set; }

        public List<byte> ChannelData = new();
        public Dictionary<string, int> JsrNamesAndPositions = new();
        public Dictionary<string, string> NamedAsmBlocks = new();

        public SoundEffectData() { }
    }
}
