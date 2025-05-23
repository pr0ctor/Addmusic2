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
        public void AddToCache(string fileName, string filePath);
        public void AddToCache(string fileName, FileStream fileData);

        public object GetFileData(string fileName);
    }
}
