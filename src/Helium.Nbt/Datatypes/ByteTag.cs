using System;
using System.Numerics;

namespace Helium.Nbt.Datatypes
{
    public class ByteTag : NumericTag<Byte>
    {
        public override BigInteger GetAsBigInteger()
            => new BigInteger(Value);

        public override Byte GetAsByte()
            => Value;

        public override Decimal GetAsDecimal()
            => Value;

        public override Double GetAsDouble()
            => Value;

        public override Single GetAsFloat()
            => Value;

        public override Int16 GetAsInt16()
            => Value;

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
