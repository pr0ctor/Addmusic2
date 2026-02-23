using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Sample
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public List<byte> Data { get; set; } = [];
        public ushort LoopPoint { get; set; }
        public bool IsImportant { get; set; }
        public bool IsLooping { get; set; }
        public bool IsBNK { get; set; }

        public Sample()
        {
            LoopPoint = 0;
            IsImportant = true;
            IsBNK = false;
        }
    }
}
