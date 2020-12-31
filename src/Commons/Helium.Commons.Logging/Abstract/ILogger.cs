using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging.Abstract
{
    public interface ILogger
    {
        public void LogMessage(String message);
        public void LogMessage(LogLevel level, String message);
        public void LogMessage(EventData data, LogLevel level, String message);

        public void LogDebug(String message);
        public void LogDebug(EventData data, String message);

        public void LogTrace(String message);
        public void LogTrace(EventData data, String message);

        public void LogInformation(String message);
        public void LogInformation(EventData data, String message);

        public void LogWarning(String message);
        public void LogWarning(EventData data, String message);

        public void LogError(String message);
        public void LogError(EventData data, String message);

        public void LogCritical(String message);
        public void LogCritical(EventData data, String message);

        public void LogFatal(String message);
        public void LogFatal(EventData data, String message);
    }
}
