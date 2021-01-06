using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Block
{
    public record MaterialColor
    {
        public UInt16 Id { get; init; }
        public UInt32 Color { get; init; }
    }
}
