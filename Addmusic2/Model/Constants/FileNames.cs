using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    public static class FileNames
    {
        public static class ExecutionLocations
        {
            public static readonly string InstallLocation = AppDomain.CurrentDomain.BaseDirectory;
            public static readonly string ExecutionLocation = Environment.CurrentDirectory;
        }

        public static class ConfigurationFiles
        {
            // Original Files
            public static readonly string SongList = "Addmusic_list.txt";
            public static readonly string SampleGroups = "Addmusic_sample groups.txt";
            public static readonly string SoundEffects = "Addmusic_sound effects.txt";
            public static readonly string AddmusicOptionsTxt = "Addmusic_options.txt";

            // New Files
            public static readonly string AddmusicOptionsJson = "AddmusicOptions.json";
            public static readonly string AddmusicSongListJson = "AddmusicSongSfxList.json";
            public static readonly string AddmusicSampleGroupsJson = "AddmusicSampleGroups.json";
            public static readonly string AddmusicSoundEffectsJson = "AddmusicSoundEffects.json";
        }

        public static class StaticFiles
        {
            public static readonly string TempTextFile = "temp" + FileExtensions.TextFile;
            public static readonly string TempLogFile = "temp" + FileExtensions.LogFile;
            public static readonly string TempAsmFile = "temp" + FileExtensions.Asm;
            public static readonly string TempBinFile = "temp" + FileExtensions.BinPatchData;
            public static readonly string EmptyBrr = "EMPTY" + FileExtensions.SampleBrr;
            public static string GetEmptyBrrLocation()
            {
                var initialLocation = ExecutionLocations.InstallLocation;
                return Path.Combine(initialLocation, FolderNames.SamplesBase, EmptyBrr);
            }
        }
        
        public static class FolderNames
        {
            public static readonly string AsmBase = "asm";
            public static readonly string AsmSNES = "SNES";
            public static readonly string AsmSNESBin = "bin";
            public static readonly string Sfx1DF9 = "1DF9";
            public static readonly string Sfx1DFC = "1DFC";
            public static readonly string MusicBase = "music";
            public static readonly string MusicOriginal = "originals";
            public static readonly string MusicCustom = "custom";
            public static readonly string SamplesBase = "samples";
            public static readonly string SamplesDefault = "default";
            public static readonly string SamplesOptimized = "optimized";

            public static readonly string LogFolder = "logs";

            public static List<string> GetInitialDirectories()
            {
                var initialLocation = ExecutionLocations.InstallLocation;

                var initialOriginalMusicData = Path.Combine(initialLocation, FileNames.FolderNames.MusicBase, FileNames.FolderNames.MusicOriginal);
                var initial1DF9Data = Path.Combine(initialLocation, FileNames.FolderNames.Sfx1DF9);
                var initial1DFCData = Path.Combine(initialLocation, FileNames.FolderNames.Sfx1DFC);
                var initialSamplesDefaultData = Path.Combine(initialLocation, FileNames.FolderNames.SamplesBase, FileNames.FolderNames.SamplesDefault);
                var initialSamplesOptimizedData = Path.Combine(initialLocation, FileNames.FolderNames.SamplesBase, FileNames.FolderNames.SamplesOptimized);
                var initialAsmData = Path.Combine(initialLocation, FileNames.FolderNames.AsmBase);
                var initialAsmSNESData = Path.Combine(initialLocation, FileNames.FolderNames.AsmBase, FileNames.FolderNames.AsmSNES);
                var initialAsmBinData = Path.Combine(initialLocation, FileNames.FolderNames.AsmBase, FileNames.FolderNames.AsmSNES, FileNames.FolderNames.AsmSNESBin);
                return new List<string>
                {
                    initialOriginalMusicData,
                    initial1DF9Data,
                    initial1DFCData,
                    initialSamplesDefaultData,
                    initialSamplesOptimizedData,
                    initialAsmData,
                    initialAsmSNESData,
                    initialAsmBinData,
                };
            }
        }

        public static class FileExtensions
        {
            public static readonly string Asm = ".asm";
            public static readonly string BinPatchData = ".bin";
            public static readonly string TextFile = ".txt";
            public static readonly string LogFile = ".log";
            public static readonly string SampleBrr = ".brr";
            public static readonly string SampleBank = ".bnk";
            public static readonly List<string> ValidSampleExtensions = new List<string>()
            {
                SampleBrr,
                SampleBank,
            };
        }
    }
}
