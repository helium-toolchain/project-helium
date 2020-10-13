using System;
using System.Collections.Generic;
using System.Text;

namespace Helium.Utility.Chat
{
    [Flags]
    public enum ChatFormatting
    {
        Black = 1,
        DarkBlue = 2,
        DarkGreen = 4,
        Cyan = 8,
        DarkRed = 16,
        DarkPurple = 32,
        Orange = 64,
        Gray = 128,
        Blue = 256,
        Green = 1024,
        Aqua = 2048,
        Red = 4096,
        LightPurple = 8192,
        Yellow = 16384,
        White = 32768,

        Obfuscated = 65536,
        Bold = 131072,
        Strikethrough = 262144,
        Underline = 524288,
        Italic = 1048576
    }
}
