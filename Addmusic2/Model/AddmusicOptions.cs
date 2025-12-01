using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class AddmusicOptions
    {
        [JsonProperty("romName")]
        public string? RomName { get; set; }
        [JsonProperty("enableConversion")]
        public bool? EnableConversion {  get; set; }
        [JsonProperty("enableEchoBufferCheck")]
        public bool? EnableEchoCheck { get; set; }
        [JsonProperty("enableBankOptimizations")]
        public bool? EnableBankOptimizations { get; set; }
        [JsonProperty("aggressiveFreespace")]
        public bool? EnableAggressiveFreespace { get; set; }
        [JsonProperty("retainDuplicateSamples")]
        // Deprecate this option. Do not impliment
        public bool? RetainDuplicateSamples { get; set; }
        [JsonProperty("validateHexCommmands")]
        public bool? ValidateHexCommands { get; set; }
        [JsonProperty("generatePatches")]
        public bool? GeneratePatches { get; set; }
        [JsonProperty("enableSampleOptimization")]
        public bool? EnableSampleOpimizations { get; set; }
        [JsonProperty("enableSA1Addressing")]
        public bool? EnableSA1Addressing { get; set; }
        [JsonProperty("loggingSettings")]
        public LoggingSettings? LoggingSettings { get; set; }
        [JsonProperty("exportSfx")]
        public bool? ExportSfx { get; set; }
        [JsonProperty("generateVisualization")]
        public bool? GenerateVisualization { get; set; }

    }

    internal class LoggingSettings
    {
        // Type of logs to display
        [JsonProperty("logToFile")]
        public bool LogToFile { get; set; } = false;
        // Type of logs to display
        [JsonProperty("loggingLevel")]
        public string LoggingLevel { get; set; } = nameof(LogLevel.Information);
        // Where to pipe the log data to
        //      can be either a path to a file or to a folder 
        [JsonProperty("logLocation")]
        public string LogLocation { get; set; }
    }
}
