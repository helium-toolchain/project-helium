using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Helium.Nbt.Datatypes
{
    public class UInt16Tag : NumericTag<UInt16>
    {
        public override BigInteger GetAsBigInteger()
            => new BigInteger(Value);

        public override Byte GetAsByte()
            => (Byte)Value;

        public override Decimal GetAsDecimal()
            => Value;

        public override Double GetAsDouble()
            => Value;

        public override Single GetAsFloat()
            => Value;

        public override Int16 GetAsInt16()
            => (Int16)Value;

        public override Int32 GetAsInt32()
            => Value;

        public override Int64 GetAsInt64()
            => Value;

        public override UInt16 GetAsUInt16()
            => Value;

        public override UInt32 GetAsUInt32()
            => Value;

        public override UInt64 GetAsUInt64()
            => Value;

        public override ValueType GetAsValueType()
            => Value;
    }
}
