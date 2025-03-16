using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface ICLArgs
    {
        public void ParseArguments(IConfiguration config, string[] args);
        public string GenerateHelp();
    }
}
