using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Helpers
{
    internal static class PatchBuilders
    {
        public static string HexWidthFormat(string value, int amount) => value.ToUpperInvariant().PadLeft(amount, '0');

        public static string BuildSoundEffectAsmPatch(int aramPosition, string asmData) => $@"norom
arch spc700

org $000000
incsrc ""asm/main.asm""
base ${aramPosition:X4}


org $008000

{asmData}

";

        public static string SongListPointerName(string number) => $"SGPointer{number}";

        public static string SfxTable0Contents = "\r\nincbin \"SFX1DF9Table.bin\"\r\n";
        public static string SfxTable1Contents = "\r\nincbin \"SFX1DFCTable.bin\"\r\nincbin \"SFXData.bin\"\r\n";

        public static string SongSampleListXkasOverride = "db $53, $54, $41, $52\t\t\t\t; Needed to stop Asar from treating this like an xkas patch.\n";
        public static string SongSampleGroupPointerLabel = "dw SGEnd-SampleGroupPtrs-$01\ndw SGEnd-SampleGroupPtrs-$01^$FFFF\nSampleGroupPtrs:\n\n";
        public static string SongSampleListEndLabel = "SGEnd:";

    }
}
