using Addmusic2.Model.Constants;
using Addmusic2.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model.Interfaces
{
    internal interface IAddmusicLogger
    {
        public void UpdateLogLocation(string logFilePath);
        public void LogInformation(LogLevel level, string message, bool alwaysLog = false);
        public void LogWarning(LogLevel level, string message, bool alwaysLog = false);
        public void LogError(LogLevel level, string message, bool alwaysLog = false);
        public void LogToFile(LogLevel level, string message);
        public void LogToConsole(LogLevel level, string message);
        public string GenerateLogMessage(LogLevel level, string message, string severity);
        public LogLevel GetCurrentLogLevel();
    }
}
