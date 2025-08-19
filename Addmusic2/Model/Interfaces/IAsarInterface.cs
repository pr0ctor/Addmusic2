using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsarCLR;

namespace Addmusic2.Model.Interfaces
{
    internal interface IAsarInterface : IDisposable
    {
        public Asar191 Asar { get; set; }
        public bool InitializeAsar();
        public bool CloseAsar();
        public string GetAsarVersion();
        public string GetAsarApiVersion();
    }
}
