using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Utility.Types
{
    public struct BlockPosition
    {
        private Byte x, z;

        public Byte X {
            get => x;
            set {
                if(value >= 16)
                    throw new ArgumentException("In-Chunk X coordinate cannot exceed 15 (range: 0 - 15)", nameof(X));
                x = value;
            }
        }

        public Byte Y { get; set; }

        public Byte Z {
            get => z;
            set {
                if(Z >= 16)
                    throw new ArgumentException("In-Chunk Z coordinate cannot exceed 15 (range: 0 - 15)", nameof(Z));
                z = value;
            }
        }
    }
}
