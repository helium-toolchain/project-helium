using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using static Helium.Utility.Chat.ChatFormattingConverter;

namespace Helium.Nbt.Datatypes
{
    public class CharTag : AbstractTag
    {
        public Char Value { get; internal set; }

        internal override AbstractTag Copy()
            => this;

        internal override void Write(Stream writer)
        {
            writer.Write(Encoding.UTF8.GetBytes(
                $"{GetChatFormattingString(SyntaxFormatting_Key)}{Key}: " +
                $"{GetChatFormattingString(SyntaxFormatting_String)}{Value}"));
        }

        internal StringTag ConvertToStringTag()
            => new StringTag
            {
                Key = this.Key,
                Value = $"{this.Value}"
            };
    }
}
