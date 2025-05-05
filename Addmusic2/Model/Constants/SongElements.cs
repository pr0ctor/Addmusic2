using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class SongElements
    {
        public static List<string> ValidSpecialDirectives = [
            "spc",
            "instruments",
            "samples",
            "pad",
            "define",
            "undef",
            "ifdef",
            "ifndef",
            "endif",
            "louder",
            "tempoimmunity",
            "path",
            "am4",
            "amm",
            "amk=",
            "halvetempo",
            "option",
            "smwvtable",
            "nspcvtable",
            "noloop",
        ];

        public static List<char> ValidNoteCharacters = [
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'r',
        ];

        public static List<char> ValidNoteSymbols = [
            '+',
            '-',
        ];

        public static List<char> ValidChannelNumbers = [
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
        ];

        public static List<char> ValidControlSymbols = [
            'v', // volume
            'w', // global volume
            'h', // tune
            'o', // octave
            'l', // default length
            'y', // pan
            'q', // quantization
            't', // tempo
            'p', // vibrato
            'n', // noise
        ];
    }
}
