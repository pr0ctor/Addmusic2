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
            public static readonly string SongList = "Addmusic_list.txt";
            public static readonly string SampleGroups = "Addmusic_sample groups.txt";
            public static readonly string SoundEffects = "Addmusic_sound effects.txt";
        }
        
        public static class FolderNames
        {
            public static readonly string Sfx1DF9 = "1DF9";
            public static readonly string Sfx1DFC = "1DFC";
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
