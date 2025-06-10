using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.Localization;
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
        private MessageService _messageService { get; set; }
        private List<Argument> ValidArgs { get; }

        public string RomName { get; set; } = string.Empty;
        public bool Convert { get; set; } = true;
        public bool CheckEcho { get; set; } = true;
        public int BankStart { get; set; } = MagicNumbers.DefaultValues.DefaultBankStart;
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

        public CLArgs(MessageService messageService)
        {
            _messageService = messageService;
            ValidArgs = new List<Argument>
            {
                // Rom Name
                new Argument(_messageService.GetCLArgRomNameNameMessage(), 50, false, _messageService.GetCLArgRomNameDescriptionMessage(), ["--r","--rom"]),
                // Convert
                new Argument(_messageService.GetCLArgConvertOldAddmusicNameMessage(), 100, false, _messageService.GetCLArgConvertOldAddmusicDescriptionMessage(), ["--c"]),
                // Check Echo
                new Argument(_messageService.GetCLArgCheckEchoNameMessage(), 200, false, _messageService.GetCLArgCheckEchoDescriptionMessage(), ["--e"]),
                // Bank Start
                new Argument(_messageService.GetCLArgBankStartNameMessage(), 300, false, _messageService.GetCLArgBankStartDescriptionMessage(), ["--b"]),
                // Verbose Logging
                new Argument(_messageService.GetCLArgVerboseLoggingNameMessage(), 400, false, _messageService.GetCLArgVerboseLoggingDescriptionMessage(), ["--v","--verbose"]),
                // Aggressive Freespace
                new Argument(_messageService.GetCLArgAggressiveFreeSpaceNameMessage(), 500, false, _messageService.GetCLArgAggressiveFreeSpaceDescriptionMessage(), ["--a"]),
                // Duplicate Checking
                new Argument(_messageService.GetCLArgDuplicateCheckingNameMessage(), 600, false, _messageService.GetCLArgDuplicateCheckingDescriptionMessage(), ["--d"]),
                // Hex Validation
                new Argument(_messageService.GetCLArgHexValidationNameMessage(), 700, false, _messageService.GetCLArgHexValidationDescriptionMessage(), ["--h"]),
                // Create Patch
                new Argument(_messageService.GetCLArgCreatePatchNameMessage(), 800, false, _messageService.GetCLArgCreatePatchDescriptionMessage(), ["--p"]),
                // Optimize Sample Usage
                new Argument(_messageService.GetCLArgOptimizeSampleUsageNameMessage(), 900, false, _messageService.GetCLArgOptimizeSampleUsageDescriptionMessage(), ["--u"]),
                // Allow SA1
                new Argument(_messageService.GetCLArgAllowSA1NameMessage(), 1000, false, _messageService.GetCLArgAllowSA1DescriptionMessage(), ["--s"]),
                // Dump Sound Effects
                new Argument(_messageService.GetCLArgDumpSoundEffectsNameMessage(), 1100, false, _messageService.GetCLArgDumpSoundEffectsDescriptionMessage(), ["--dumpsfx","--sfxdump"]),
                // Visualize
                new Argument(_messageService.GetCLArgVisualizeSPCNameMessage(), 1200, false, _messageService.GetCLArgVisualizeSPCDescriptionMessage(), ["--visualize"]),
                // Remove First Use Notification
                new Argument(_messageService.GetCLArgRemoveFirstUseNameMessage(), 1300, false, _messageService.GetCLArgRemoveFirstUseDescriptionMessage(), ["--noblock"], false),
                // Name
                new Argument(_messageService.GetCLArgStreamDirectNameMessage(), 1400, false, _messageService.GetCLArgStreamDirectDescriptionMessage(), ["--srd","--streamredirect"]),
                // Generate SPC
                new Argument(_messageService.GetCLArgGenerateSPCNameMessage(), 1500, false, _messageService.GetCLArgGenerateSPCDescriptionMessage(), ["--norom"]),
                // Help
                new Argument(_messageService.GetCLArgHelpNameMessage(), 1600, false, _messageService.GetCLArgHelpDescriptionMessage(), ["--?","--help"]),
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
                        BankStart = MagicNumbers.DefaultValues.DefaultBankStartFromCLArgs;
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
                        // todo fix and localize this exception
                        throw new InvalidOperationException("Undefined Command Line Argument. Add definitions.");
                }
            }
        }

        public string GenerateHelp()
        {
            var builder = new StringBuilder();
            // todo localize this message
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
