using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using static System.Math;

namespace Helium.Utility.Chat
{
    public static class ChatFormattingConverter
    {
        public static String GetChatFormattingString(ChatFormatting flags, Boolean ResetBefore = false)
        {
            String returnValue = "";
            Int32 Value, Formatting = (Int32)flags;

            if (ResetBefore)
                returnValue += "§r";

            for(Byte c = 0; Formatting != 0; c++)
            {
                Value = (Int32)Pow(2, c);

                if((Formatting & Value) == Value)
                {
                    Formatting ^= Value;
                    try
                    {
                        returnValue += GetChatFormattingInstance(Value);
                    } catch {
                        //TODO: actually log the error :b
                    }
                }
            }

            return returnValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static String GetChatFormattingInstance(Int32 value)
        {
            return value switch
            {
                1 => "§0",
                2 => "§1",
                4 => "§2",
                8 => "§3",
                16 => "§4",
                32 => "§5",
                64 => "§6",
                128 => "§7",
                256 => "§8",
                512 => "§9",
                1024 => "§a",
                2048 => "§b",
                4096 => "§c",
                8196 => "§d",
                16384 => "§e",
                32768 => "§f",

                65536 => "§k",
                131072 => "§l",
                262144 => "§m",
                524288 => "§n",
                1048576 => "§o",

                _ => throw new ArgumentException("The color code passed to private Helium.Utility.Chat.ChatFormattingConverter.GetChatFormattingInstance" +
                "(Int32) is not allowed")
            };
        }
    }
}
