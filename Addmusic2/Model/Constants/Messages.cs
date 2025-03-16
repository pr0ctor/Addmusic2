using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class Messages
    {
        #region Intro Messages
        public static class IntroMessages
        {
            public static readonly string AddmusicVersion = $"";
            public static readonly string ParserVersion = $"";
            public static readonly string ReadTheReadMe = "Protip: Be sure to read the readme! If there's an error or something doesn't\nseem right, it may have your answer!\n\n";
        }
        #endregion



        #region Error Messages

        public static class GenericErrorMessages
        {
            public static string MissingRequiredArguments(List<string> required) => $"Missing the following required arguments: {string.Join(", ", required)}";
        }

        #endregion
    }
}
