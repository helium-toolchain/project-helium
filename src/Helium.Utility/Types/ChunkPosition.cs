using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Utility.Types
{
    public struct ChunkPosition
    {
        private Byte x, z;

        public Byte X {
            get => x;
            set {
                if(value >= 32)
                    throw new ArgumentException("In-Region X coordinate cannot exceed 31 (range: 0 - 31)", nameof(X));
                x = value;
            }
        }

        public Byte Z {
            get => z;
            set {
                if(value >= 32)
                    throw new ArgumentException("In-Region Z coordinate cannot exceed 31 (range: 0 - 31)", nameof(Z));
                z = value;
            }
        }
    }
}
