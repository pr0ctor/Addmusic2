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
using Antlr4.Runtime;
using Microsoft.Extensions.Logging;

namespace Addmusic2.Logic
{
    internal class AddmusicLogic : IAddmusicLogic
    {
        private ILogger<IAddmusicLogic> _logger;
        private MessageService _messageService;
        private FileCachingService _fileService;

        public DateTime LastModification { get; set; }

        public AddmusicLogic(ILogger<IAddmusicLogic> logger, MessageService messageService, FileCachingService fileCachingService)
        {
            _logger = logger;
            _messageService = messageService;
            _fileService = fileCachingService;
        }

        public void Run()
        {
            var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

            var songData = PreProcessSong(fileData);
            ProcessSong(songData);

            PostProcessSong();
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

    }
}
