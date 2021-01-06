using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Serialization.Models
{
    public class ChunkSectionModel
    {
        public Boolean IsAir { get; set; }
        public Byte YLevel { get; set; }
        public BlockModel[,,] Blocks { get; set; }
    }
}
