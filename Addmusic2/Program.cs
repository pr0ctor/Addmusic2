// See https://aka.ms/new-console-template for more information
using Addmusic2.Helpers;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

Console.WriteLine("Hello, World!");

var clArgs = new CLArgs();

// clargs or load rom file
if (File.Exists("Addmusic_options.txt"))
{

}
else
{
    IConfiguration config = new ConfigurationBuilder()
        .AddCommandLine(args)
        .Build();

    if (config["--?"] != null || config["--help"] != null)
    {
        clArgs.GenerateHelp();
        return;
    }

    clArgs.ParseArguments(config, args);
}


var globalSettings = new GlobalSettings();

var startTime = DateTime.Now;

Console.WriteLine(Messages.IntroMessages.AddmusicVersion);
Console.WriteLine(Messages.IntroMessages.ParserVersion);
Console.WriteLine(Messages.IntroMessages.ReadTheReadMe);

// load Asar here


/*using var logFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("Addmusic2.Program", (clArgs.Verbose) ? LogLevel.Debug : LogLevel.Information)
        .AddConsole();
});

var logger = logFactory.CreateLogger<IAddmusicLogic>();*/

// Set up Dependency Injection

var services = new ServiceCollection();

services.AddLocalization();

services.AddLogging(builder => builder.AddConsole());

services.AddScoped<IRomOperations, RomOperations>();

services.AddSingleton<IAsarInterface>();
services.AddSingleton<ICLArgs>(clArgs);

// Load Necessary file data into Cache
var cachingServie = new FileCachingService();
cachingServie.InitializeCache();

services.AddSingleton<IFileCachingService>(cachingServie);

var addmusic = services.BuildServiceProvider();