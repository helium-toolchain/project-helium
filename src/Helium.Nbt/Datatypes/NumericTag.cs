using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

using static Helium.Utility.Chat.ChatFormattingConverter;

namespace Helium.Nbt.Datatypes
{
    public abstract class NumericTag<T> : AbstractTag
    {
        public T Value { get; internal set; }

        public abstract ValueType GetAsValueType();

        public abstract Byte GetAsByte();
        public abstract Int16 GetAsInt16();
        public abstract UInt16 GetAsUInt16();
        public abstract Int32 GetAsInt32();
        public abstract UInt32 GetAsUInt32();
        public abstract Int64 GetAsInt64();
        public abstract UInt64 GetAsUInt64();
        public abstract BigInteger GetAsBigInteger();

        public abstract Single GetAsFloat();
        public abstract Decimal GetAsDecimal();
        public abstract Double GetAsDouble();

        internal override void Write(Stream writer)
        {
            writer.Write(Encoding.UTF8.GetBytes(
                $"{GetChatFormattingString(SyntaxFormatting_Key)}{Key}: " +
                $"{GetChatFormattingString(SyntaxFormatting_Number)}{Value}"));
        }
    }
}
