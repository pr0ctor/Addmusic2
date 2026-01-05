using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Services
{
    internal class AddmusicLogger : IAddmusicLogger, IDisposable
    {
        public readonly AddmusicLoggerOptions _options;
        private FileStream? _logFile;
        private StreamWriter? _logFileWriter;

        private readonly LogLevel _LogLevel;

        private readonly string InfoIndicator = "INFO";
        private readonly string WarningIndicator = "WARN";
        private readonly string ErrorIndicator = "ERROR";

        public AddmusicLogger(AddmusicLoggerOptions options)
        {
            _options = options;

            _LogLevel = options.LoggingLevel;
            
            // handle opening the file to log to, if applicable
            if(options.LogToFile == true)
            {
                UpdateLogLocation(options.LogFilePath);
            }
        }

        public void UpdateLogLocation(string logFilePath)
        {
            // get the custom location for the log file
            //      this can be a URI to a folder or a URI to a specific file
            var fileLocation = logFilePath.Trim();

            // generate the name and location of the default file
            var now = DateTime.UtcNow;
            var baseFileName = $"{FileNames.LogFiles.GeneralLogFileBase}-{now:yyyyMMdd}";
            var fileName = baseFileName + FileNames.FileExtensions.TextFile;
            var filePath = FileNames.LogFiles.GeneralLogFile(baseFileName);

            // check to see if the given location is a file or if the current default filename is found at that location
            //      if that file is not found, set the location to be the default file name at either the custom or default location
            if (fileLocation.Length > 0 &&
                !Directory.Exists(fileLocation) &&
                !File.Exists(fileLocation) && 
                !File.Exists(Path.Combine(fileLocation, fileName)))
            {
                fileLocation = (fileLocation != null && fileLocation.Length > 0)
                    ? Path.Combine(fileLocation, fileName)
                    : filePath;
            }

            // open the file at the location
            _logFile = File.OpenWrite(fileLocation);

            var streamWriter = new StreamWriter(_logFile);

            _logFileWriter = streamWriter;
        }

        public void ReleaseResources()
        {
            _logFileWriter?.Dispose();
            _logFile?.Dispose();
        }

        public void Dispose()
        {
            ReleaseResources();
        }

        private bool IsEnabled(LogLevel logLevel)
        {
            if (_options.Enable == true)
            {
                return true;
            }

            if (((int)logLevel) <= ((int)_LogLevel))
            {
                return true;
            }

            return false;
        }

        public string GenerateLogMessage(LogLevel level, string message, string severity)
        {
            var now = DateTime.UtcNow;
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"[{now:u}] ");
            messageBuilder.Append($"[{severity.ToUpper()}] ");
            messageBuilder.Append(message);

            return messageBuilder.ToString();
        }

        public string FormatLineColumnCharacter(string line, string column)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"Line: {line} ");
            messageBuilder.Append($"Column: {column} ");
            
            return messageBuilder.ToString();
        }

        public void LogToConsole(LogLevel level, string message)
        {
            Console.WriteLine(message);
        }

        public void LogToFile(LogLevel level, string message)
        {
            _logFileWriter?.WriteLine(message);
        }

        public void LogInformation(LogLevel level, string message, bool alwaysLog = false)
        {
            if(!IsEnabled(level) && alwaysLog == false)
            {
                return;
            }

            LogMessage(level, message, InfoIndicator);
        }

        public void LogWarning(LogLevel level, string message, bool alwaysLog = false)
        {
            if (!IsEnabled(level) && alwaysLog == false)
            {
                return;
            }

            LogMessage(level, message, WarningIndicator);
        }

        public void LogError(LogLevel level, string message, bool alwaysLog = false)
        {
            if (!IsEnabled(level) && alwaysLog == false)
            {
                return;
            }

            LogMessage(level, message, ErrorIndicator);
        }

        private void LogMessage(LogLevel level, string message, string severity)
        {
            var fullMessage = GenerateLogMessage(level, message, severity);

            LogToConsole(level, fullMessage);

            if (_options.LogToFile)
            {
                LogToFile(level, fullMessage);
            }
        }

        public LogLevel GetCurrentLogLevel()
        {
            return _LogLevel;
        }
        
    }
}
