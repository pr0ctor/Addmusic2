// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;

Console.WriteLine("Hello, World!");

var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

var stream = CharStreams.fromString(fileData);

var lexer = new MmlLexer(stream);
var tokenStream = new CommonTokenStream(lexer);
var parser = new MmlParser(tokenStream);

var songContext = parser.song();

var x = 1;