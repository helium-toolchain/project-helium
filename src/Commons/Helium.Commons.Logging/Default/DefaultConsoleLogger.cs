using Helium.Commons.Logging.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging.Default
{
    public class DefaultConsoleLogger : ILogger
    {
        /// <summary>
        /// The minimal log level required to post the entry to console
        /// </summary>
        public LogLevel MinimalLevel { get; set; }

        public void LogMessage(String message) 
            => LogInformation(message);

        public void LogMessage(LogLevel level, String message)
            => LogMessage(new EventData(0, 0, 0, 0, null), level, message);

        public void LogMessage(EventData data, LogLevel level, String message)
        {
            switch(level) {
                case LogLevel.Debug:
                    LogDebug(data, message);
                    break;
                case LogLevel.Trace:
                    LogTrace(data, message);
                    break;
                case LogLevel.Info:
                    LogInformation(data, message);
                    break;
                case LogLevel.Warning:
                    LogWarning(data, message);
                    break;
                case LogLevel.Error:
                    LogError(data, message);
                    break;
                case LogLevel.Critical:
                    LogCritical(data, message);
                    break;
                case LogLevel.Fatal:
                    LogFatal(data, message);
                    break;

                default:
                    throw new ArgumentException("The specified LogLevel was not recognized", nameof(level));
            }
        }


        public void LogDebug(String message) 
            => LogDebug(new EventData(0, 0, 0, 0, null), message);

        public void LogDebug(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Debug.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Debug.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogTrace(String message)
            => LogTrace(new EventData(0, 0, 0, 0, null), message);
        public void LogTrace(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Trace.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Trace.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogInformation(String message)
            => LogInformation(new EventData(0, 0, 0, 0, null), message);
        public void LogInformation(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Info.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Info.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogWarning(String message)
            => LogWarning(new EventData(0, 0, 0, 0, null), message);
        public void LogWarning(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Warn.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Warn.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogError(String message)
            => LogError(new EventData(0, 0, 0, 0, null), message);
        public void LogError(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Error.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Error.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogCritical(String message)
            => LogCritical(new EventData(0, 0, 0, 0, null), message);
        public void LogCritical(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.ForegroundColor = DefaultFormatting.Crit.ForegroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Crit.LogName}]"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {data}: {message}");
        }

        public void LogFatal(String message)
            => LogFatal(new EventData(0, 0, 0, 0, null), message);
        public void LogFatal(EventData data, String message)
        {
            Console.Write($"[{DateTimeOffset.Now}] ");
            Console.BackgroundColor = DefaultFormatting.Fatal.BackgroundColor;
            Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Fatal.LogName}]"));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($" {data}: {message}");
        }
    }
}
