// See https://aka.ms/new-console-template for more information
using Addmusic2.Helpers;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Antlr4.Runtime;
using System.Text.RegularExpressions;
using Addmusic2.Visitors;
using Addmusic2.Logic;
using Addmusic2.Model.Localization;
using Addmusic2.Services;
using Newtonsoft.Json;

//[assembly: RootNamespace("Addmusic2")]

Console.WriteLine("Hello, World!");

/*var fileData = File.ReadAllText(@"Samples/Seenpoint Intro.txt");

var replacementsRegex = new Regex(@$"""([^\s=""]+)\s*=\s*([^""]+)""");

var matches = replacementsRegex.Matches(fileData);

foreach (Match match in matches)
{
    var searchValue = match.Groups[1].Value;
    var replaceValue = match.Groups[2].Value;

    fileData = fileData.Replace(searchValue, replaceValue);
}

var stream = CharStreams.fromString(fileData);

var lexer = new MmlLexer(stream);
var tokenStream = new CommonTokenStream(lexer);
var parser = new MmlParser(tokenStream);
var newParser = new AdvMmlVisitor();

var songContext = parser.song();

var songNodeTree = newParser.VisitSong(songContext);

var x = 1;*/
// bad and dirty way to get early localization
var tempService = new ServiceCollection();
tempService.AddLocalization(options =>
{
    options.ResourcesPath = "Localization";
});
tempService.AddTransient<MessageService>();

var tempSericeProvider = tempService.BuildServiceProvider();

var tempMessageService = tempSericeProvider.GetRequiredService<MessageService>();

var clArgs = new CLArgs(tempMessageService);

// Always check and parse command line arguments
IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

// If user is using the help command in any section of the args, show help and quit
//      Don't process anything
if (config["--?"] != null || config["--help"] != null)
{
    clArgs.GenerateHelp();
    return;
}

// clargs or load rom file

var addmusicSettings = new AddmusicOptions();

if(File.Exists(FileNames.ConfigurationFiles.AddmusicOptionsJson))
{
    var optionsFileData = File.ReadAllText(FileNames.ConfigurationFiles.AddmusicOptionsJson);
    addmusicSettings = JsonConvert.DeserializeObject<AddmusicOptions>(optionsFileData);
}
//else if(File.Exists(FileNames.ConfigurationFiles.AddmusicOptionsTxt))
//{
//    // do new file conversion process
//    var optionsFileData = File.ReadAllText(FileNames.ConfigurationFiles.AddmusicOptionsTxt);
//    addmusicSettings = FileConverters.ConvertTxtOptionsToJsonOptions(optionsFileData);
//}
else // dont support converting the old format over
{
    // no configs found
    //      throw error or create new file or something with defaults
}

clArgs.ParseArguments(config, args);

// get rid of the temp service provider
tempSericeProvider.Dispose();

var globalSettings = new GlobalSettings();

globalSettings.ReconcileFileSettingsAndCLArgs(addmusicSettings, clArgs);

var startTime = DateTime.Now;

// load Asar here


using var logFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("Addmusic2.Program", (globalSettings.Verbose) ? LogLevel.Debug : LogLevel.Information)
        .AddConsole();
});

var logger = logFactory.CreateLogger<IAddmusicLogic>();

// Set up Dependency Injection

var services = new ServiceCollection();

services.AddLocalization(options =>
{
    options.ResourcesPath = "Localization";
});
services.AddTransient<MessageService>();

services.AddLogging(builder => builder.AddConsole());

services.AddScoped<IRomOperations, RomOperations>();

// services.AddSingleton<IAsarInterface>();
services.AddSingleton<IGlobalSettings>(globalSettings);
services.AddSingleton<IFileCachingService, FileCachingService>();
services.AddSingleton<IAddmusicLogic, AddmusicLogic>();

var serviceProvider = services.BuildServiceProvider();

var addmusicLogic = serviceProvider.GetRequiredService<IAddmusicLogic>();
var messageService = serviceProvider.GetRequiredService<MessageService>();
var fileService = serviceProvider.GetRequiredService<IFileCachingService>();

// Load Necessary file data into Cache
fileService.InitializeCache();

logger.LogInformation(messageService.GetIntroAddmusicVersionMessage());
logger.LogInformation(messageService.GetIntroParserVersionMessage());
logger.LogInformation(messageService.GetIntroReadTheReadMeMessage());

/*Console.WriteLine(Messages.IntroMessages.AddmusicVersion);
Console.WriteLine(Messages.IntroMessages.ParserVersion);
Console.WriteLine(Messages.IntroMessages.ReadTheReadMe);*/

addmusicLogic.Run();