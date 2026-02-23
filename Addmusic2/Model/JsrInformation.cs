using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class JsrInformation
    {
        public string JsrName { get; set; } = string.Empty;
        public byte[] JsrData { get; set; }
        public int SequencePosition { get; set; }
        public int ChannelPosition { get; set; }
    }
}
