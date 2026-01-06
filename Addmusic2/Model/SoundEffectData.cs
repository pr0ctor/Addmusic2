using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class SoundEffectData
    {

        public string Name { get; set; } = string.Empty;
        public string PatchData { get; set; } = string.Empty;

        public int AramPosition { get; set; }

        public List<byte> ChannelData = new();
        public Dictionary<int, string> JsrPositionsAndNames = new();
        public Dictionary<string, string> NamedAsmBlocks = new();
        public Dictionary<string, byte[]> CompiledAsmCodeBlocks = new();
        public List<JsrInformation> JsrInformation = new();

        public SoundEffectData() { }
    }
}
