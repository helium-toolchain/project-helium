﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Helium.Nbt.Datatypes
{
    public class DoubleTag : NumericTag<Double>
    {
        public override BigInteger GetAsBigInteger()
            => new BigInteger((Int64)Value);

        public override Byte GetAsByte()
            => (Byte)Value;

        public override Decimal GetAsDecimal()
            => (Decimal)Value;

        public override Double GetAsDouble()
            => Value;

        public override Single GetAsFloat()
            => (Single)Value;

        public override Int16 GetAsInt16()
            => (Int16)Value;

        public override Int32 GetAsInt32()
            => (Int32)Value;

        public override Int64 GetAsInt64()
            => (Int64)Value;

        public override UInt16 GetAsUInt16()
            => (UInt16)Value;

        public override UInt32 GetAsUInt32()
            => (UInt32)Value;

        public override UInt64 GetAsUInt64()
            => (UInt64)Value;

        public override ValueType GetAsValueType()
            => Value;
    }
}