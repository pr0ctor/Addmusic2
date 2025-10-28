using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class ExtractedAsmDataNames
    {
        public static class PatchAsmLocationNames
        {
            public static readonly string ProgramUploadPositionText = "!DefARAMRet = ";
            public static readonly string ProgramBasePositionText = "base ";
            public static readonly string ExpARAMRetText = "!ExpARAMRet = ";
            public static readonly string DefARAMRetText = "!DefARAMRet = ";
            public static readonly string SongCountText = "!SongCount = ";
            public static readonly string MusicPointersText = "MusicPtrs:";
            public static readonly string MainLoopPositionText = "MainLoopPos: ";
            public static readonly string ReuploadPositionText = "ReuploadPos: ";
            public static readonly string SFXTable0Text = "SFXTable0:";
            public static readonly string SFXTable1Text = "SFXTable1:";
            public static readonly string GlobalMusicCountText = "!GlobalMusicCount = #";
        }

        public static class AdditionalValues
        {
            public static readonly string NoSFXIsEnabled = "NoSFX is enabled";
        }
        

    }
}
