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

        // Adds the file to the Cache and returns the size of the file
        public int AddToCache(string fileName, string filePath);

        // Adds the file to the Cache and returns the size of the file
        public int AddToCache(string fileName, Stream fileData);

        public MemoryStream? GetFromCache(string fileName);

        public (bool IsFound, string Filename) CheckCacheContains(string fileName);
    }
}
