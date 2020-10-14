using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Helium.Utility.Chat;

using static Helium.Utility.Chat.ChatFormattingConverter;

namespace Helium.Nbt.Datatypes
{
    public class StringTag : AbstractTag
    {
        public String Value { get; internal set; }

        internal override AbstractTag Copy()
            => this;

        internal override void Write(Stream writer)
        {
            writer.Write(Encoding.UTF8.GetBytes(
                $"{GetChatFormattingString(SyntaxFormatting_Key, true)}{Key}{GetChatFormattingString(0, true)}: " +
                $"\"{GetChatFormattingString(SyntaxFormatting_String, true)}{Value}{GetChatFormattingString(0, true)}\""));
        }
    }
}
