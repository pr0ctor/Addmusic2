using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Exceptions
{
    internal class MissingFileDataException : Exception
    {
        public MissingFileDataException()
        {
        }

        public MissingFileDataException(string message)
            : base(message)
        {
        }

        public MissingFileDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
