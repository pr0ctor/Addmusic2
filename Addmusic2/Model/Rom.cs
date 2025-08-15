using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Rom : IRomData
    {
        public string RomFileName { get; set; }
        public string RomFilePath { get; set; }
        public List<byte[]> RomHeader { get; set; }
        public FileStream RomData { get; set; }

        public Rom()
        {

        }

        public void LoadRomFile(string path, string fileName)
        {
            if(!fileName.Contains(".smc") && !fileName.Contains(".sfc"))
            {
                //if()
            }

            if(!File.Exists(path))
            {
                throw new FileNotFoundException();
            }


        }
    }
}
