using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Addmusic2.Model;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
using Addmusic2.Parsers;
using Addmusic2.Services;
using Addmusic2.Visitors;
using Addmusic2.Model.Constants;
using Antlr4.Runtime;
using Microsoft.Extensions.Logging;

namespace Addmusic2.Logic
{
    internal class AddmusicLogic : IAddmusicLogic
    {
        private ILogger<IAddmusicLogic> _logger;
        private MessageService _messageService;
        private FileCachingService _fileService;
        private GlobalSettings _globalSettings;

        public DateTime LastModification { get; set; }

        public AddmusicLogic(ILogger<IAddmusicLogic> logger, MessageService messageService, FileCachingService fileCachingService, GlobalSettings globalSettings)
        {
            _logger = logger;
            _messageService = messageService;
            _fileService = fileCachingService;
            _globalSettings = globalSettings;
        }

        public void Run()
        {

            LoadRequiredSampleGroups();



            var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

            var songData = PreProcessSong(fileData);
            ProcessSong(songData);

            PostProcessSong();
        }

        public void RunSingleSong(string fileData)
        {

        }

        public string PreProcessSong(string fileData)
        {
            var replacementsRegex = new Regex(@$"""([^\s=""]+)\s*=\s*([^""]+)""");

            var matches = replacementsRegex.Matches(fileData);

            foreach (Match match in matches)
            {
                var searchValue = match.Groups[1].Value;
                var replaceValue = match.Groups[2].Value;

                fileData = fileData.Replace(searchValue, replaceValue);
            }

            return fileData;
        }

        public void ProcessSong(string fileData)
        {
            var stream = CharStreams.fromString(fileData);

            var lexer = new MmlLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new MmlParser(tokenStream);
            var mmlVisitor = new AdvMmlVisitor();

            var songContext = parser.song();

            var rootNode = mmlVisitor.VisitSong(songContext);

            // potentially logic for changing parsers

            var songParser = new SongParser(_logger, _messageService);

            var song = new Song(songParser, rootNode)
            {
                SongText = fileData,
            };

            song.ParseSong();
        }

        public void PostProcessSong()
        {

        }


        private void LoadRequiredSampleGroups()
        {
            var sampleGroups = _globalSettings.ResourceList.SampleGroups;

            var defaultGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesDefault, StringComparison.InvariantCultureIgnoreCase));
            var optimizedGroup = sampleGroups.FindAll(g => g.Name.Equals(FileNames.FolderNames.SamplesOptimized, StringComparison.InvariantCultureIgnoreCase));

            if(defaultGroup.Count > 1)
            {
                // handle more than one "default" group
            }
            else if(defaultGroup.Count == 0)
            {
                // handle missing "default" group
            }

            if (optimizedGroup.Count > 1)
            {
                // handle more than one "optimized" group
            }
            else if(optimizedGroup.Count == 0)
            {
                // handle missing "optimized" group
            }

            Helpers.Helpers.LoadSampleGroupToCache(_fileService, defaultGroup.First());
            Helpers.Helpers.LoadSampleGroupToCache(_fileService, optimizedGroup.First());
        }
    }
}
