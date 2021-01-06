using Helium.Utility.Types;
using Helium.World.Environment.Block;
using Helium.World.Environment.Block.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Serialization.Models
{
    public class BlockModel
    {
        public BlockPosition Position { get; set; }
        public Int32 RegistryIdentifier { get; set; }
        public IBlockState[] BlockStates { get; set; }
    }
}
