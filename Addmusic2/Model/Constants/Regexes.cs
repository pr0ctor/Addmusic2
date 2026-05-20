using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    public partial class Regexes
    {
        [GeneratedRegexAttribute(@$"""([^\s=""]+)\s*=\s*([^""]+)""")]
        public static partial Regex ReplacementParameterRegex();

        [GeneratedRegexAttribute(@"([a-fA-f0-9]{1,2})\s*([\*\?]{0,2})\s*(.*)")]
        public static partial Regex SfxListFileLine();

        [GeneratedRegexAttribute(@"([a-gA-G])(\+|\-)?\=?([0-9]*)(\.*)(\^\=?[0-9]+\.*)*")]
        public static partial Regex NoteRegex();

        [GeneratedRegexAttribute(@"([rR])\=?([0-9]*)(\.*)(\^\=?[0-9]+\.*)*")]
        public static partial Regex RestRegex();

        [GeneratedRegexAttribute(@"\^\=?([0-9]+)*\.*")]
        public static partial Regex TieRegex();
    }
}
