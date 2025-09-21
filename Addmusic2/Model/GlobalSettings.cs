using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class GlobalSettings : IGlobalSettings
    {
        public AddmusicSongSfxResources ResourceList { get; set; } = new();

        #region Runtime Arguments

        public string RomName { get; set; } = string.Empty;
        public bool EnableConversion { get; set; } = true;
        public bool EnableEchoCheck { get; set; } = true;
        public bool EnableBankOptimizations { get; set; } = false;
        public int BankStart { get; set; } = MagicNumbers.DefaultValues.DefaultBankStart;
        public bool Verbose { get; set; } = false;
        public bool EnableAggressiveFreespace { get; set; } = false;
        // deprecate this option
        public bool RetainDuplicateSamples { get; set; } = true;
        public bool ValidateHexCommands { get; set; } = true;
        public bool GeneratePatches { get; set; } = false;
        public bool EnableSampleOpimizations { get; set; } = true;
        public bool EnableSA1Addressing { get; set; } = true;
        public bool ExportSfx { get; set; } = false;
        public bool GenerateVisualization { get; set; } = false;
        public bool ForceNoContinuePopup { get; set; } = false;
        public bool RedirectStandardStreams { get; set; } = false;
        public bool GenerateSPC { get; set; } = false;

        #endregion

        #region Extracted Asm Data

        public int ProgramSize { get; set; }
        public int ProgramUploadPosition { get; set; }
        public string ProgramReuploadPosition { get; set; } = string.Empty;
        public string MusicPointersPosition { get; set; } = string.Empty;
        public string MainLoopPosition { get; set; } = string.Empty;
        public int ExARAMRet { get; set; }
        public int DefARAMRet { get; set; }
        public int SongCount { get; set; }
        public string SfxTable0 { get; set; } = string.Empty;
        public string SfxTable1 { get; set; } = string.Empty;


        #endregion


        public int GlobalSongMaxIndex { get; set; }

        public GlobalSettings() { }

        public GlobalSettings(AddmusicOptions fileOptions, CLArgs clArgs)
        {
            ReconcileFileSettingsAndCLArgs(fileOptions, clArgs);
        }

        public void LoadAddusicSongSfxResourceLists()
        {
            var initialDirectory = FileNames.ExecutionLocations.InstallLocation;
            // OG file
            var songFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.SongList);
            var sampleGroupFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.SampleGroups);
            var sfxFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.SoundEffects);
            // New Json Files
            var songJsonFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.AddmusicSongListJson);
            var sampleGroupJsonFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.AddmusicSampleGroupsJson);
            var sfxJsonFileLocation = Path.Combine(initialDirectory, FileNames.ConfigurationFiles.AddmusicSoundEffectsJson);

            if(File.Exists(songJsonFileLocation))
            {
                var songJsonFile = File.ReadAllText(songJsonFileLocation);
                var songJson = JsonConvert.DeserializeObject<AddmusicSongList>(songJsonFile);
                ResourceList.Songs = songJson;
            }
            else
            {
                var ogSongFile = File.ReadAllText(songFileLocation);
                var parsedData = Helpers.FileConverters.ConvertToAddmusicSongList(ogSongFile);

                // todo add logic to write out the new json file before leaving this codeblock

                ResourceList.Songs = parsedData;
            }

            if (File.Exists(sampleGroupJsonFileLocation))
            {
                var sampleGroupJsonFile = File.ReadAllText(sampleGroupJsonFileLocation);
                var sampleGroupJson = JsonConvert.DeserializeObject<List<AddmusicSampleGroup>>(sampleGroupJsonFile);
                ResourceList.SampleGroups = sampleGroupJson;
            }
            else
            {
                var ogSampleGroupFile = File.ReadAllText(sampleGroupFileLocation);
                var parsedData = Helpers.FileConverters.ConverToAddmusicSampleGroups(ogSampleGroupFile);

                // todo add logic to write out the new json file before leaving this codeblock

                ResourceList.SampleGroups = parsedData;
            }

            if(File.Exists(sfxJsonFileLocation))
            {
                var sfxJsonFile = File.ReadAllText(sfxJsonFileLocation);
                var parsedData = JsonConvert.DeserializeObject<AddmusicSfxList>(sfxJsonFile);

                // todo add logic to write out the new json file before leaving this codeblock

                ResourceList.SoundEffects = parsedData;
            }
            else
            {
                var ogSFXFile = File.ReadAllText(sfxFileLocation);
                var parsedData = Helpers.FileConverters.ConvertToAddmusicSfxList(ogSFXFile);

                ResourceList.SoundEffects = parsedData;
            }

        }

        // Determine which settings to use
        //      If a setting is given through the command line, prefer that value
        //      If a value exists in the options file, use that value
        //      Otherwise, use the defaults listed in the options file and this class
        public void ReconcileFileSettingsAndCLArgs(AddmusicOptions fileOptions, CLArgs clArgs)
        {
            if(clArgs.RomName != null)
            {
                RomName = clArgs.RomName;
            }
            else if(fileOptions.RomName != null)
            {
                RomName = fileOptions.RomName;
            }

            if(clArgs.Convert != null)
            {
                EnableConversion = (bool)clArgs.Convert;
            }
            else if(fileOptions.EnableConversion != null)
            {
                EnableConversion = (bool)fileOptions.EnableConversion;
            }

            if(clArgs.CheckEcho != null)
            {
                EnableEchoCheck = (bool)clArgs.CheckEcho;
            }
            else if(fileOptions.EnableEchoCheck != null)
            {
                EnableEchoCheck = (bool)fileOptions.EnableEchoCheck;
            }

            // double check bank optimizations

            if(clArgs.Aggressive != null)
            {
                EnableAggressiveFreespace = (bool)clArgs.Aggressive;
            }
            else if(fileOptions.EnableAggressiveFreespace != null)
            {
                EnableAggressiveFreespace = (bool)fileOptions.EnableAggressiveFreespace;
            }

            if(clArgs.DuplicateCheck != null)
            {
                RetainDuplicateSamples = (bool)clArgs.DuplicateCheck;
            }
            else if(fileOptions.RetainDuplicateSamples != null)
            {
                RetainDuplicateSamples = (bool)fileOptions.RetainDuplicateSamples;
            }

            if (clArgs.ValidateHex != null)
            {
                ValidateHexCommands = (bool)clArgs.ValidateHex;
            }
            else if (fileOptions.ValidateHexCommands != null)
            {
                ValidateHexCommands = (bool)fileOptions.ValidateHexCommands;
            }

            if(clArgs.DoNotPatch != null)
            {
                GeneratePatches = (bool)clArgs.DoNotPatch;
            }
            else if(fileOptions.GeneratePatches != null)
            {
                GeneratePatches = (bool)fileOptions.GeneratePatches;
            }

            if(clArgs.OptimizeSampleUsage != null)
            {
                EnableSampleOpimizations = (bool)clArgs.OptimizeSampleUsage;
            }
            else if(fileOptions.EnableSampleOpimizations != null)
            {
                EnableSampleOpimizations = (bool)fileOptions.EnableSampleOpimizations;
            }

            if(clArgs.AllowSA1 != null)
            {
                EnableSA1Addressing = (bool)clArgs.AllowSA1;
            }
            else if(fileOptions.EnableSA1Addressing != null)
            {
                EnableSA1Addressing = (bool)fileOptions.EnableSA1Addressing;
            }

            if(clArgs.SFXDump != null)
            {
                ExportSfx = (bool)clArgs.SFXDump;
            }
            else if(fileOptions.ExportSfx != null)
            {
                ExportSfx = (bool)fileOptions.ExportSfx;
            }

            if(clArgs.VisualizeSongs != null)
            {
                GenerateVisualization = (bool)clArgs.VisualizeSongs;
            }
            else if(fileOptions.GenerateVisualization != null)
            {
                GenerateVisualization = (bool)fileOptions.GenerateVisualization;
            }

            if(clArgs.ForceNoContinuePopup != null)
            {
                ForceNoContinuePopup = (bool)clArgs.ForceNoContinuePopup;
            }
            //else if(fileOptions.)
            //{

            //}

            if(clArgs.VisualizeSongs != null)
            {
                GenerateVisualization = (bool)clArgs.VisualizeSongs;
            }
            else if(fileOptions.GenerateVisualization != null)
            {
                GenerateVisualization = (bool)fileOptions.GenerateVisualization;
            }
        }
    }
}
