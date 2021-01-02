using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Utility.Types
{
    public struct TripleUInt6
    {
        private Int32 values { get; set; }

        public TripleUInt6(Int32 TotalValue)
        {
            values = TotalValue;
        }

        public TripleUInt6(Byte a, Byte b, Byte c)
        {
            values = (a << 14) + (b << 7) + c;
        }

        public Int32 GetFirst() 
            => (values & 0b111111100000000000000) >> 14;

        public Int32 GetSecond() 
            => (values & 0b000000011111110000000) >> 7;

        public Int32 GetThird() 
            => values & 0b000000000000001111111;

        public void SetFirst(Byte First)
        {
            if(First > 64)
                throw new ArgumentException("TripleUInt6 values cannot exceed 64", nameof(First));

            values &= 0b000000011111111111111;
            values |= (First << 14);
        }

        public void SetSecond(Byte Second)
        {
            if(Second > 64)
                throw new ArgumentException("TripleUInt6 values cannot exceed 64", nameof(Second));

            values &= 0b111111100000001111111;
            values |= (Second << 7);
        }

        public void SetThird(Byte Third)
        {
            if(Third > 64)
                throw new ArgumentException("TripleUInt6 values cannot exceed 64", nameof(Third));

            values &= 0b000000000000001111111;
            values |= Third;
        }
    }
}
