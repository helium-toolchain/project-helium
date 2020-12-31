using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging
{
    /// <summary>
    /// Part of the Helium Toolchain API. Indicates additional metadata for each log level.
    /// </summary>
    public record LogFormatting
    {
        public ConsoleColor ForegroundColor { get; init; }
        public ConsoleColor BackgroundColor { get; init; }
        public String LogName { get; init; }
    }
}
