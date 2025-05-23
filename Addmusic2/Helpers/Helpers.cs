using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model;
using Microsoft.Extensions.Configuration;
using static Addmusic2.Model.SongTree.NotePayload;

namespace Addmusic2.Helpers
{
    internal static class Helpers
    {

        public static string ParseAccidentalToString(Accidentals accidental) => accidental switch
        {
            Accidentals.None => "",
            Accidentals.Sharp => "+",
            Accidentals.Flat => "-",
            _ => throw new ArgumentOutOfRangeException("Invalid Accidental.")
        };

        public static bool isDigits(string s)
        {
            if (s == null || s == "") return false;

            for (int i = 0; i < s.Length; i++)
                if ((s[i] ^ '0') > 9)
                    return false;

            return true;
        }

    }
}
