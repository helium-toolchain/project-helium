using Helium.Utility.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Shapes
{
    public struct PrimitiveShape
    {
        public TripleUInt6 CornerA { get; set; }
        public TripleUInt6 CornerB { get; set; }

        public PrimitiveShape(TripleUInt6 CornerA, TripleUInt6 CornerB)
        {
            this.CornerA = CornerA;
            this.CornerB = CornerB;
        }
    }
}
