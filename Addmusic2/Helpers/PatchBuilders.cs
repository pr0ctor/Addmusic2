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

        public static string BuildSoundEffectAsmPatch(string aramPosition, string asmData) => $@"norom
arch spc700

org $000000
incsrc ""asm/main.asm""
base ${PatchBuilders.HexWidthFormat(aramPosition, 4)}


org $008000

{asmData}

";


    }
}
