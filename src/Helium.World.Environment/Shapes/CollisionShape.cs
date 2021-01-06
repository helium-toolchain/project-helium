using Helium.Utility.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Shapes
{
    public class CollisionShape : IShape
    {
        public PrimitiveShape[] Primitives { get; set; }
        public Boolean IsColliding(BlockPosition ShapePosition, BlockPosition CollidingPosition, IShape CollidingShape)
        {
            throw new NotImplementedException();
        }
    }
}
