using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Services
{
    internal class FileCachingService : IFileCachingService
    {

        //public Dictionary<string, FileStream>;

        public void AddToCache(string fileName, string filePath)
        {
            throw new NotImplementedException();
        }

        public void AddToCache(string fileName, FileStream fileData)
        {
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }

        public object GetFileData(string fileName)
        {
            throw new NotImplementedException();
        }

        public void InitializeCache()
        {
            throw new NotImplementedException();
        }
    }
}
