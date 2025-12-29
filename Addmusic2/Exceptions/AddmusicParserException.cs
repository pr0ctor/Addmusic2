using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Exceptions
{
    internal class AddmusicParserException : Exception
    {
        public AddmusicParserException()
        {
        }

        public AddmusicParserException(string message)
            : base(message)
        {
        }

        public AddmusicParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
