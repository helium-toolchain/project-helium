using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Shapes
{
    public interface IShape
    {
        public PrimitiveShape[] Primitives { get; set; }
    }
}
