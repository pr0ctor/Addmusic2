using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Constants
{
    internal static class Messages
    {
        #region Misc

        public static readonly string DefaultSpcGameName = "Super Mario World (custom)";

        #endregion


        #region Intro Messages
        public static class IntroMessages
        {
            public static readonly string AddmusicVersion = $"";
            public static readonly string ParserVersion = $"";
            public static readonly string ReadTheReadMe = "Protip: Be sure to read the readme! If there's an error or something doesn't\nseem right, it may have your answer!\n\n";
        }
        #endregion

        #region Warning Messages

        public static class WarningMessages
        {
            public static readonly string DefaultLengthValidationWarning = "WARNING: A default note length was used that is not divisible by 192 ticks, and thus results in a fractional tick value.";
        }

        #endregion


        #region Error Messages

        public static class GenericErrorMessages
        {
            public static string MissingRequiredArguments(List<string> required) => $"Missing the following required arguments: {string.Join(", ", required)}";

            public static string DefaultLengthOutOfRange(int minValue, int maxValue, int foundValue) => $"Illegal Default Length value ({foundValue}) found. Value must be between {minValue} and {maxValue} . ";
        }

        #endregion
    }
}
