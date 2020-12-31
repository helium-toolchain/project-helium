using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging.Default
{
    public static class DefaultFormatting
    {
        public static readonly LogFormatting Debug = new LogFormatting
        {
            LogName = "Debug",
            ForegroundColor = ConsoleColor.DarkGreen,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Trace = new LogFormatting
        {
            LogName = "Trace",
            ForegroundColor = ConsoleColor.Green,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Info = new LogFormatting
        {
            LogName = "Info",
            ForegroundColor = ConsoleColor.DarkMagenta,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Warn = new LogFormatting
        {
            LogName = "Warn",
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Error = new LogFormatting
        {
            LogName = "Error",
            ForegroundColor = ConsoleColor.Red,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Crit = new LogFormatting
        {
            LogName = "Crit",
            ForegroundColor = ConsoleColor.DarkRed,
            BackgroundColor = ConsoleColor.Black
        };

        public static readonly LogFormatting Fatal = new LogFormatting
        {
            LogName = "Fatal",
            ForegroundColor = ConsoleColor.White,
            BackgroundColor = ConsoleColor.DarkRed
        };
    }
}
