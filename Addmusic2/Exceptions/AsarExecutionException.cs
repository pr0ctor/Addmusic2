using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Exceptions
{
    internal class AsarExecutionException : Exception
    {
        public AsarExecutionException()
        {
        }

        public AsarExecutionException(string message)
            : base(message)
        {
        }

        public AsarExecutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
