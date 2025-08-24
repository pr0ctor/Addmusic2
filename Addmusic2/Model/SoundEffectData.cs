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
        public Dictionary<int, string> JsrPositionsAndNames = new();
        public Dictionary<string, string> NamedAsmBlocks = new();
        public Dictionary<string, byte[]> CompiledAsmCodeBlocks = new();
        public List<JsrInformation> JsrInformation = new();

        public SoundEffectData() { }
    }

    internal class JsrInformation
    {
        public string JsrName { get; set; } = string.Empty;
        public byte[] JsrData { get; set; }
        public int SequencePosition { get; set; }
    }
}
