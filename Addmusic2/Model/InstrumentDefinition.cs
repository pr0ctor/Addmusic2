using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class InstrumentDefinition
    {
        public enum InstrumentType
        {
            Sample,
            Number,
            Noise,
        }

        public string SampleName { get; set; }
        public ISongNode InstrumentNumber { get; set; }
        public ISongNode NoiseData { get; set; }
        public InstrumentType Type { get; set; }

        public List<string> HexSettings { get; set; }

        public InstrumentDefinition() { }
    }
}
