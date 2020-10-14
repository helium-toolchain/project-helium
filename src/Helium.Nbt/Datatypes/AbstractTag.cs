using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Helium.Utility.Chat;

namespace Helium.Nbt.Datatypes
{
    /// <summary>
    /// Our Pure Serializer has ascended. Beyond lies only the refuse and regret of its creation.
    /// We shall enter this namespace no longer.
    /// <para>
    ///     Seriously, dont use Helium.Nbt.Datatypes directly. Please.
    /// </para>
    /// </summary>
    public abstract class AbstractTag
    {
        protected static ChatFormatting SyntaxFormatting_Key = ChatFormatting.Aqua;
        protected static ChatFormatting SyntaxFormatting_String = ChatFormatting.Green;
        protected static ChatFormatting SyntaxFormatting_Number = ChatFormatting.Orange;
        protected static ChatFormatting SyntaxFormatting_Type = ChatFormatting.Red;
        public UInt16 Identifier { get; protected set; }
        public String Key { get; internal set; }


        internal virtual void Write(Stream writer)
            => throw new IOException("Cannot serialize AbstractTag instance");

        internal virtual AbstractTag Copy()
            => this;
    }
}
