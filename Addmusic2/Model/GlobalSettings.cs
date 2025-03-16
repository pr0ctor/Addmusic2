using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class GlobalSettings : IGlobalSettings
    {
        public bool UsingSA1 { get; set; }
        public bool IsAggressive { get; set; }
    }
}
