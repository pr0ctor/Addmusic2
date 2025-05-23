using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class CLArgs : ICLArgs
    {
        private List<Argument> ValidArgs { get; }

        public string RomName { get; set; }
        public bool Convert { get; set; } = true;
        public bool CheckEcho { get; set; } = true;
        public int BankStart { get; set; } = MagicNumbers.DefaultBankStart;
        public bool Verbose { get; set; } = false;
        public bool Aggressive { get; set; } = false;
        public bool DuplicateCheck { get; set; } = true;
        public bool ValidateHex { get; set; } = true;
        public bool DoNotPatch { get; set; } = false;
        public bool OptimizeSampleUsage { get; set; } = true;
        public bool AllowSA1 { get; set; } = true;
        public bool SFXDump { get; set; } = false;
        public bool VisualizeSongs { get; set; } = false;
        public bool ForceNoContinuePopup { get; set; } = false;
        public bool RedirectStandardStreams { get; set; } = false;
        public bool GenerateSPC { get; set; } = false;

        public CLArgs()
        {
            ValidArgs = new List<Argument>
            {
                new Argument("ROM Name", 50, false, "The Name of the ROM to modify.", ["--r","--rom"]),
                new Argument("Convert", 100, false, "Force off conversion from Addmusic 4.05 and AddmusicM", ["--c"]),
                new Argument("Check Echo", 200, false, "Turn off echo buffer checking.", ["--e"]),
                new Argument("Bank Start", 300, false, "Do not attempt to save music data in bank 0x40 and above.", ["--b"]),
                new Argument("Verbose Logging", 400, false, "Turn verbosity on.  More information will be displayed while using this.", ["--v","--verbose"]),
                new Argument("Aggresive Free Space", 500, false, "Make free space finding more aggressive.", ["--a"]),
                new Argument("Duplicate Checking", 600, false, "Turn off duplicate sample checking.", ["--d"]),
                new Argument("Hex Validation", 700, false, "Turn off hex command validation.", ["--h"]),
                new Argument("Create Patch", 800, false, "Create a patch, but do not patch it to the ROM.", ["--p"]),
                new Argument("Optimize Sample Usage", 900, false, "Turn off Optimize Sample Usage.", ["--u"]),
                new Argument("Allow SA1", 1000, false, "Turn off allowing SA1 enabled features.", ["--s"]),
                new Argument("Dump Sound Effects", 1100, false, "Dump sound effects", ["--dumpsfx","--sfxdump"]),
                new Argument("Visualize", 1200, false, "Generates a visualization of the SPC.", ["--visualize"]),
                new Argument("Remove First Use Notification", 1300, false, "Removes the first use notification.", ["--noblock"], false),
                new Argument("Name", 1400, false, "desc", ["--srd","--streamredirect"]),
                new Argument("Generate SPC", 1500, false, "Only generate SPC files, no ROM required.", ["--norom"]),
                new Argument("Help", 1600, false, "Lists and shows help information for the various commands.", ["--?","--help"]),
            };
        }

        public void ParseArguments(IConfiguration config, string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException(Messages.GenericErrorMessages.MissingRequiredArguments(ValidArgs.Where(a => a.IsRequired).Select(a => a.Aliases.First()).ToList()));
            }

            var allCommands = new List<string>();
            ValidArgs.ForEach(a => allCommands.AddRange(a.Aliases));

            foreach(var (arg, value) in config.AsEnumerable())
            {
                switch (arg)
                {
                    case "--r":
                    case "--rom":
                        RomName = value ?? "";
                        break;
                    case "--b":
                        BankStart = 0x080000;
                        break;
                    case "--c":
                        Convert = false;
                        break;
                    case "--e":
                        CheckEcho = false;
                        break;
                    case "--d":
                        DuplicateCheck = false;
                        break;
                    case "--h":
                        ValidateHex = false;
                        break;
                    case "--u":
                        OptimizeSampleUsage = false;
                        break;
                    case "--s":
                        AllowSA1 = false;
                        break;
                    case "--v":
                    case "--verbose":
                        Verbose = true;
                        break;
                    case "--a":
                        Aggressive = true;
                        break;
                    case "--p":
                        DoNotPatch = true;
                        break;
                    case "--dumpsfx":
                    case "--sfxdump":
                        SFXDump = true;
                        break;
                    case "--visualize":
                        VisualizeSongs = true;
                        break;
                    case "--noblock":
                        ForceNoContinuePopup = true;
                        break;
                    case "--srd":
                    case "--streamredirect":
                        RedirectStandardStreams = true;
                        break;
                    case "--norom":
                        GenerateSPC = true;
                        break;
                    case "--?":
                    case "--help":
                        break;
                    default:
                        throw new InvalidOperationException("Undefined Command Line Argument. Add definitions.");
                }
            }
        }

        public string GenerateHelp()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Options:");

            foreach(var argument in ValidArgs.Where(a => a.DisplayInHelp == true).OrderBy(a => a.Order))
            {
                builder.AppendLine($"{argument.Name}");
                builder.AppendLine($"\t[{string.Join(", ", argument.Aliases)}]: {argument.Description}");
            }

            return builder.ToString();
        }
    }
}
