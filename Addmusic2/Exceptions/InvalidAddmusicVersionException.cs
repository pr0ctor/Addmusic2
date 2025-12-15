using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Exceptions
{
    internal class InvalidAddmusicVersionException : Exception
    {
        public InvalidAddmusicVersionException()
        {
        }

        public InvalidAddmusicVersionException(string message)
            : base(message)
        {
        }

        public InvalidAddmusicVersionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
