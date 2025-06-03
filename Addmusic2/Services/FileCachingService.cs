using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Services
{
    internal class FileCachingService : IFileCachingService
    {
        //public Dictionary<string, FileStream>;
        private ILogger<IAddmusicLogic> _logger;

        // Dict(Set<FileName>, FileData>
        // FileHash is needed for collisions due to samples with identical names but different data contents
        private Dictionary<HashSet<string>, MemoryStream> _cache = 
            new Dictionary<HashSet<string>, MemoryStream>(HashSet<string>.CreateSetComparer());

        // Dict(MD5 Hash, FileName)
        // MD5 hashes of all of the files in the Cache with their associated filename
        private Dictionary<string, string> _fileHashes = new Dictionary<string, string>();

        public FileCachingService(ILogger<IAddmusicLogic> logger) 
        {
            _logger = logger;
        }

        public void InitializeCache()
        {
            throw new NotImplementedException();
        }

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

        private bool IsFileInCache(string fileName)
        {
            if(fileName == null || fileName.Length == 0)
            {
                return false;
            }


        }
    }
}
