using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class InstrumentInformation
    {

        public int InstrumentNumber { get; set; }
        public string InstrumentSample { get; set; }
        public List<int> HexComponents { get; set; } = new();

        public InstrumentInformation() { }

    }
}
