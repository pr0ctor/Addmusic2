using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addmusic2.Model;
using Addmusic2.Model.Constants;
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

        public static string ParseSongListItemTypeToString(SongListItemType itemType) => itemType switch
        {
            SongListItemType.Original => FileNames.FolderNames.MusicOriginal,
            SongListItemType.Custom => FileNames.FolderNames.MusicCustom,
            SongListItemType.UserDefined => "UserDefined",
            SongListItemType.NA => "",
            _ => throw new ArgumentOutOfRangeException("Invalid SongListItemType.")
        };

        public static string ParseSfxListItemTypeToString(SfxListItemType itemType) => itemType switch
        {
            SfxListItemType.Sfx1DF9 => FileNames.FolderNames.Sfx1DF9,
            SfxListItemType.Sfx1DFC => FileNames.FolderNames.Sfx1DFC,
            SfxListItemType.UserDefined => "UserDefined",
            SfxListItemType.NA => "",
            _ => throw new ArgumentOutOfRangeException("Invalid SfxListItemType.")
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
