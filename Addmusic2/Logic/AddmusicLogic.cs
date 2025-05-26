using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Addmusic2.Model;
using Addmusic2.Model.Interfaces;
using Addmusic2.Visitors;
using Antlr4.Runtime;

namespace Addmusic2.Logic
{
    internal class AddmusicLogic : IAddmusicLogic
    {
        public DateTime LastModification { get; set; }

        public AddmusicLogic()
        {

        }

        public void Run()
        {
            var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

            var songData = PreProcessSong(fileData);
            ProcessSong(songData);
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

            var song = new Song(rootNode)
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
