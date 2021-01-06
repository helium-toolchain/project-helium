using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.World.Environment.Serialization.Models
{
    public class RegionModel
    {
        public ChunkModel[,] Chunks { get; set; }
    }
}
