using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Sample
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public ushort LoopPoint { get; set; }
        public bool Exists { get; set; }
        public bool Important { get; set; }
        public bool IsBNK { get; set; }

        public Sample()
        {
            LoopPoint = 0;
            Exists = false;
            Important = true;
            IsBNK = false;
        }
    }
}
