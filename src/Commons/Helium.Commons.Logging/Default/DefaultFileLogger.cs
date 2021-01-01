using Helium.Commons.Logging.Abstract;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging.Default
{
    public class DefaultFileLogger : ILogger
    {
        /// <summary>
        /// All log calls will log into this file
        /// </summary>
        public StreamWriter FileStream { get; set; }

        /// <summary>
        /// Create a new FileLogger instance using the name of the logfile
        /// </summary>
        public DefaultFileLogger(String Filename)
        {
            FileStream = new StreamWriter(Filename);
        }

        /// <summary>
        /// Create a new FileLogger instance using an already existing stream
        /// </summary>
        public DefaultFileLogger(Stream FileStream)
        {
            this.FileStream = new StreamWriter(FileStream);
        }

        public void LogMessage(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Info, message);
        public void LogMessage(LogLevel level, String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), level, message);
        public void LogMessage(EventData data, LogLevel level, String message)
        {
            FileStream.Write($"[{DateTimeOffset.Now}] ");
            FileStream.Write(String.Format("{0:-5}", $"[{formatting[level].LogName}]"));
            FileStream.WriteLine($" {data}: {message}");
            FileStream.Flush();
        }

        public void LogDebug(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Debug, message);
        public void LogDebug(EventData data, String message)
            => LogMessage(data, LogLevel.Debug, message);

        public void LogTrace(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Trace, message);
        public void LogTrace(EventData data, String message)
            => LogMessage(data, LogLevel.Trace, message);

        public void LogInformation(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Info, message);
        public void LogInformation(EventData data, String message) 
            => LogMessage(data, LogLevel.Info, message);

        public void LogWarning(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Warning, message);
        public void LogWarning(EventData data, String message) 
            => LogMessage(data, LogLevel.Warning, message);

        public void LogError(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Error, message);
        public void LogError(EventData data, String message) 
            => LogMessage(data, LogLevel.Error, message);

        public void LogCritical(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Critical, message);
        public void LogCritical(EventData data, String message) 
            => LogMessage(data, LogLevel.Critical, message);

        public void LogFatal(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Fatal, message);
        public void LogFatal(EventData data, String message) 
            => LogMessage(data, LogLevel.Fatal, message);


        private static readonly Dictionary<LogLevel, LogFormatting> formatting = new Dictionary<LogLevel, LogFormatting>
        {
            { LogLevel.Debug, DefaultFormatting.Debug },
            { LogLevel.Trace, DefaultFormatting.Trace },
            { LogLevel.Info, DefaultFormatting.Info },
            { LogLevel.Warning, DefaultFormatting.Warn },
            { LogLevel.Error, DefaultFormatting.Error },
            { LogLevel.Critical, DefaultFormatting.Crit },
            { LogLevel.Fatal, DefaultFormatting.Fatal }
        };

        ~DefaultFileLogger()
        {
            FileStream.Close();
        }
    }
}
