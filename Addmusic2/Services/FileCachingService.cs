using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Services
{
    internal class FileCachingService : IFileCachingService
    {
        private ILogger<IAddmusicLogic> _logger;

        // Dict(FileName, FileData>)
        // FileHash is needed for collisions due to samples with identical names but different data contents
        private Dictionary<string, MemoryStream> _cache = new();

        // Dict(MD5 Hash, FileName)
        // MD5 hashes of all of the files in the Cache with their associated filename
        private Dictionary<string, string> _fileHashes = new();

        // Dict(FileName, List<FileNames with same MD5 Hash>)
        // Dictionary storing files that have identical data
        //      Only stores duplicates, does not store singletons
        private Dictionary<string, List<string>> _duplicateAliases = new();

        public FileCachingService(ILogger<IAddmusicLogic> logger) 
        {
            _logger = logger;
        }

        public void InitializeCache()
        {
            //var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            // Load all:
            //      - Original Samples, Sound Effects, and Songs
            //      - Asm Files
            //      - Required .bin files
            var initialLocations = FileNames.FolderNames.GetInitialDirectories();

            foreach(var directory in initialLocations)
            {
                var directoryInfo = new DirectoryInfo(directory);
                var files = directoryInfo.GetFiles();

                foreach(var file in files)
                {
                    var fileName = "";

                    // handle special case for samples as there are by default a number of duplicates and
                    //      the specific samples' files need to be captured
                    if (directory.Contains(Path.Combine(FileNames.FolderNames.SamplesBase, FileNames.FolderNames.SamplesDefault)))
                    {
                        fileName = Path.Combine(FileNames.FolderNames.SamplesDefault, file.Name);
                    }
                    else if(directory.Contains(Path.Combine(FileNames.FolderNames.SamplesBase, FileNames.FolderNames.SamplesOptimized)))
                    {
                        fileName = Path.Combine(FileNames.FolderNames.SamplesOptimized, file.Name);
                    }
                    else
                    {
                        fileName = file.Name;
                    }

                    using var data = File.OpenRead(file.FullName);
                    AddToCache(fileName, data);
                }
            }

            // Empty Brr static file
            var emptyBrr = FileNames.StaticFiles.GetEmptyBrrLocation();
            using var emptyBrrData = File.OpenRead(emptyBrr);
            AddToCache(FileNames.StaticFiles.EmptyBrr, emptyBrrData);

        }

        public int AddToCache(string fileName, string filePath)
        {
            if(Path.Exists(filePath))
            {
                using var filedata = File.OpenRead(filePath);
                return AddToCache(fileName, filedata);
            }
            else
            {
                throw new InvalidOperationException($"Supplied Filename ( {fileName} ) at Filepath ( {filePath} ) is not found.");
            }
        }

        public int AddToCache(string fileName, Stream fileData)
        {

            var fileHash = ComputeMD5HashOfFile(fileData);

            if(_fileHashes.ContainsKey(fileHash))
            {
                var duplicateStream = new MemoryStream();
                if (_duplicateAliases.ContainsKey(fileName))
                {
                    duplicateStream = GetFromCache(fileName);
                }
                else
                {
                    var originalFileName = _fileHashes[fileHash];
                    if (_duplicateAliases.ContainsKey(originalFileName))
                    {
                        _duplicateAliases[originalFileName].Add(fileName);
                        _duplicateAliases.Add(fileName, new List<string>
                        {
                            originalFileName
                        });
                    }
                    else
                    {
                        _duplicateAliases[originalFileName] = new List<string>
                        {
                            originalFileName,
                            fileName,
                        };
                        _duplicateAliases[fileName] = new List<string>
                        {
                            originalFileName,
                        };
                    }
                    duplicateStream = GetFromCache(originalFileName);
                }

                return (int)duplicateStream!.Length;
            }
            else
            {
                _fileHashes.Add(fileHash, fileName);

                var newStream = new MemoryStream();
                fileData.Seek(0, SeekOrigin.Begin);
                fileData.CopyTo(newStream);
                _cache.Add(fileName, newStream);
                return (int)fileData.Length;
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
            _fileHashes.Clear();
            _duplicateAliases.Clear();
        }

        public MemoryStream? GetFromCache(string fileName)
        {
            if (_duplicateAliases.ContainsKey(fileName))
            {
                var originalName = _duplicateAliases[fileName].First();

                var stream = new MemoryStream();
                _cache[originalName].CopyTo(stream);

                return stream;
            }

            if(_cache.ContainsKey(fileName))
            {
                var stream = new MemoryStream();
                _cache[fileName].CopyTo(stream);

                return stream;
            }

            return null;
        }

        public ( bool IsFound, string Filename ) CheckCacheContains(string fileName)
        {
            if( _cache.ContainsKey(fileName))
            {
                return (true, fileName);
            }
            else if(_duplicateAliases.ContainsKey(fileName))
            {
                return (true, _duplicateAliases[fileName].First());
            }
            else
            {
                return (false, string.Empty);
            }
        }

        private string ComputeMD5HashOfFile(Stream dataStream)
        {
            using var md5 = MD5.Create();

            var hash = md5.ComputeHash(dataStream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

    }
}
