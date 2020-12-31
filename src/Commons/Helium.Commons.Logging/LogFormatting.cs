using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging
{
    public record LogFormatting
    {
        public LogLevel TargetLevel { get; init; }
        public ConsoleColor ForegroundColor { get; init; }
        public ConsoleColor BackgroundColor { get; init; }
        public String LogName { get; init; }
    }
}
