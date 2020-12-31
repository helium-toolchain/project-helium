using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging.Abstract
{
    /// <summary>
    /// Part of the Helium Toolchain API. Defines all necessary functionality for a Logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message at Info level.
        /// </summary>
        public void LogMessage(String message);

        /// <summary>
        /// Logs a message at the given level.
        /// </summary>
        public void LogMessage(LogLevel level, String message);

        /// <summary>
        /// Logs a message at the given level with the given event data
        /// </summary>
        public void LogMessage(EventData data, LogLevel level, String message);


        /// <summary>
        /// Logs a message at Debug level
        /// </summary>
        public void LogDebug(String message);

        /// <summary>
        /// Logs a message at Debug level with the given event data
        /// </summary>
        public void LogDebug(EventData data, String message);


        /// <summary>
        /// Logs a message at Trace level
        /// </summary>
        public void LogTrace(String message);

        /// <summary>
        /// Logs a message at Trace level with the given event data
        /// </summary>
        public void LogTrace(EventData data, String message);


        /// <summary>
        /// Logs a message at Info level
        /// </summary>
        public void LogInformation(String message);

        /// <summary>
        /// Logs a message at Info level with the given event data
        /// </summary>
        public void LogInformation(EventData data, String message);


        /// <summary>
        /// Logs a message at Warning level
        /// </summary>
        public void LogWarning(String message);

        /// <summary>
        /// Logs a message at Warning level with the given event data
        /// </summary>
        public void LogWarning(EventData data, String message);


        /// <summary>
        /// Logs a message at Error level
        /// </summary>
        public void LogError(String message);

        /// <summary>
        /// Logs a message at Error level with the given event data
        /// </summary>
        public void LogError(EventData data, String message);


        /// <summary>
        /// Logs a message at Critical level
        /// </summary>
        public void LogCritical(String message);

        /// <summary>
        /// Logs a message at Critical level with the given event data
        /// </summary>
        public void LogCritical(EventData data, String message);


        /// <summary>
        /// Logs a message at Fatal level
        /// </summary>
        public void LogFatal(String message);

        /// <summary>
        /// Logs a message at Fatal level with the given event data
        /// </summary>
        public void LogFatal(EventData data, String message);
    }
}
