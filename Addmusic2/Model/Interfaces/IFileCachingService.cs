using Addmusic2.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface IFileCachingService
    {

        public void InitializeCache();
        public void ClearCache();
        public string AddToCache(string fileName, string filePath);
        public string AddToCache(string fileName, FileStream fileData);
        public string AddToCache(string fileName, MemoryStream fileData);

        public MemoryStream GetFromCache(string fileName);
    }
}
