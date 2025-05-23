// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

var replacementsRegex = new Regex(@$"""([^\s=""]+)\s*=\s*([^""]+)""");

var matches = replacementsRegex.Matches(fileData);

foreach ( Match match in matches )
{
    var searchValue = match.Groups[1].Value;
    var replaceValue = match.Groups[2].Value;

    fileData = fileData.Replace(searchValue, replaceValue);
}

var stream = CharStreams.fromString(fileData);

var lexer = new MmlLexer(stream);
var tokenStream = new CommonTokenStream(lexer);
var parser = new MmlParser(tokenStream);

var songContext = parser.song();

var firstChannel = songContext.GetChild(24);

var channelData = firstChannel.GetChild(0);

var channelContent = channelData.GetChild(1);

var firstAtomic = channelContent.GetChild(0);

var spcChildren = songContext.GetChild(1).GetChild(0).GetChild(0);

var x = 1;