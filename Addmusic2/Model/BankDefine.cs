using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class BankDefine
    {
        public string Name { get; set; } = string.Empty;
        public List<Sample> Samples { get; set; } = new();
        public List<bool> Importants { get; set; } = new();
    }
}
