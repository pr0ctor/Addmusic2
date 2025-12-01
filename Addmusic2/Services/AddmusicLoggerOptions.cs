using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Services
{
    internal class AddmusicLoggerOptions
    {
        public bool Enable { get; set; }
        public LogLevel LoggingLevel { get; set; }
        public bool LogToFile { get; set; } = false;
        public string LogFilePath { get; set; } = "";

    }
}
