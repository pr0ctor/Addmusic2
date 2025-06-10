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
        public void AddToCache(string fileName, string filePath);
        public void AddToCache(string fileName, Stream fileData);

        public MemoryStream? GetFromCache(string fileName);
    }
}
