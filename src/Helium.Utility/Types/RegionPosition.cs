using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Utility.Types
{
    public struct RegionPosition
    {
        public SimpleDimension Dimension { get; init; }

        public Int32 X { get; set; }
        public Int32 Z { get; set; }
    }
}
