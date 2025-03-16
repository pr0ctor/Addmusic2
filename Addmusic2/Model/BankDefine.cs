using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class BankDefine
    {
        public string Name { get; set; }
        public List<Sample> Samples { get; set; } = new List<Sample>();
        public List<bool> Importants { get; set; } = new List<bool>();
    }
}
