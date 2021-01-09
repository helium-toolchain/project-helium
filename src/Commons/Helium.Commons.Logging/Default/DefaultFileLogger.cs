using Helium.Commons.Logging.Abstract;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// Minimal log level required for an entry to log
        /// </summary>
        public LogLevel MinimalLevel { get; set; }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogMessage(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Info, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogMessage(LogLevel level, String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), level, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogMessage(EventData data, LogLevel level, String message)
        {
            if(level < MinimalLevel)
                return;
            FileStream.Write($"[{DateTimeOffset.Now}] ");
            FileStream.Write(String.Format("{0:-5}", $"[{formatting[level].LogName}]"));
            FileStream.WriteLine($" {data}: {message}");
            FileStream.Flush();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogDebug(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Debug, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogDebug(EventData data, String message)
            => LogMessage(data, LogLevel.Debug, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogTrace(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Trace, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogTrace(EventData data, String message)
            => LogMessage(data, LogLevel.Trace, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogInformation(String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Info, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogInformation(EventData data, String message) 
            => LogMessage(data, LogLevel.Info, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Warning, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(EventData data, String message) 
            => LogMessage(data, LogLevel.Warning, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Error, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(EventData data, String message) 
            => LogMessage(data, LogLevel.Error, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogCritical(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Critical, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogCritical(EventData data, String message) 
            => LogMessage(data, LogLevel.Critical, message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogFatal(String message) 
            => LogMessage(new EventData(0, 0, 0, 0, null), LogLevel.Fatal, message);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
