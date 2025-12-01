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
using AsarCLR.Asar191;

//[assembly: RootNamespace("Addmusic2")]

// bad and dirty way to get early localization
var tempService = new ServiceCollection();
tempService.AddLogging(builder => builder.AddConsole());
tempService.AddLocalization();
tempService.AddTransient<MessageService>();

var tempServiceProvider = tempService.BuildServiceProvider();

var tempMessageService = tempServiceProvider.GetRequiredService<MessageService>();

var clArgs = new CLArgs(tempMessageService);

// Always check and parse command line arguments
var config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

// If user is using the help command in any section of the args, show help and quit
//      Don't process anything
if (args.Any(a => a == "--?" || a == "-?" || a == "--help" || a == "-help"))
{
    Console.WriteLine(clArgs.GenerateHelp());
    return;
}

// clargs or load rom file

var addmusicSettings = new AddmusicOptions();

if(File.Exists(FileNames.ConfigurationFiles.AddmusicOptionsJson))
{
    var optionsFileData = File.ReadAllText(FileNames.ConfigurationFiles.AddmusicOptionsJson);
    addmusicSettings = JsonConvert.DeserializeObject<AddmusicOptions>(optionsFileData);
}
//else if (File.Exists(FileNames.ConfigurationFiles.AddmusicOptionsTxt))
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
tempServiceProvider.Dispose();

var globalSettings = new GlobalSettings();

globalSettings.ReconcileFileSettingsAndCLArgs(addmusicSettings, clArgs);
globalSettings.LoadAddusicSongSfxResourceLists();

var startTime = DateTime.Now;

// load Asar here
var asarLoaded = Asar.init();

// check asar loaded

// Set up Dependency Injection

var services = new ServiceCollection();

var addmusicLoggingOptions = new AddmusicLoggerOptions()
{
    Enable = globalSettings.Verbose,
    LogToFile = globalSettings.LogToFile,
    LoggingLevel = globalSettings.LoggingLevel,
    LogFilePath = globalSettings.LogLocation,
};

//var logger = new AddmusicLogger(addmusicLoggingOptions);

// needed for the localization message service
services.AddLogging(builder => builder.AddConsole());
services.AddLocalization();
services.AddTransient<MessageService>();

services.AddTransient<RomOperations>();

// services.AddSingleton<IAsarInterface>();
services.AddSingleton<IAddmusicLogger>(new AddmusicLogger(addmusicLoggingOptions));
services.AddSingleton<IGlobalSettings>(globalSettings);
services.AddSingleton<IAddmusicLogic, AddmusicLogic>();
services.AddSingleton<IFileCachingService, FileCachingService>();

var serviceProvider = services.BuildServiceProvider();

var addmusicLogic = serviceProvider.GetRequiredService<IAddmusicLogic>();
var messageService = serviceProvider.GetRequiredService<MessageService>();
var fileService = serviceProvider.GetRequiredService<IFileCachingService>();
var logger = serviceProvider.GetRequiredService<IAddmusicLogger>();

// Load Necessary file data into Cache
fileService.InitializeCache();

logger.LogInformation(LogLevel.Information, messageService.GetIntroAddmusicVersionMessage(), true);
logger.LogInformation(LogLevel.Information, messageService.GetIntroParserVersionMessage(), true);
logger.LogInformation(LogLevel.Information, messageService.GetIntroReadTheReadMeMessage(), true);
logger.LogInformation(LogLevel.Information, $"Asar Version: {Asar.version()}", true);

addmusicLogic.Run();

// unload Asar here; might have to do some Disposable stuff due to unmanged memory stuff
Asar.close();
