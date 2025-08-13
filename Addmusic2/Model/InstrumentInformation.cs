using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class InstrumentInformation : IEquatable<InstrumentInformation>
    {

        public int InstrumentNumber { get; set; }
        public int InstrumentData { get; set; } = -1;
        public string InstrumentSample { get; set; } = "";
        public List<int> HexComponents { get; set; } = new();

        public InstrumentInformation() { }

        public bool Equals(InstrumentInformation? other)
        {
            if(other == null)
            {
                return false;
            }
            return InstrumentNumber == other.InstrumentNumber;
        }
    }
}
