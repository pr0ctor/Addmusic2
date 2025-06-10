using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    public static class FileNames
    {
        public static class ConfigurationFiles
        {
            // Original Files
            public static readonly string SongList = "Addmusic_list.txt";
            public static readonly string SampleGroups = "Addmusic_sample groups.txt";
            public static readonly string SoundEffects = "Addmusic_sound effects.txt";

            // New Files
            public static readonly string AddmusicSongList = "AddmusicSongSfxList.txt";
            public static readonly string AddmusicSampleGroups = "AddmusicSampleGroups.txt";
        }

        public static class StaticFiles
        {
            public static readonly string EmptyBrr = "EMPTY.brr";
            public static string GetEmptyBrrLocation()
            {
                var initialLocation = AppDomain.CurrentDomain.BaseDirectory;
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

            public static List<string> GetInitialDirectories()
            {
                var initialLocation = AppDomain.CurrentDomain.BaseDirectory;

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
            public static readonly string SampleBrr = ".brr";
            public static readonly string SampleBank = ".bnk";
            public static readonly List<string> SampleExtensions = new List<string>()
            {
                SampleBrr,
                SampleBank,
            };
        }
    }
}
