using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Data.Datapack.Compat
{
    public class MinecraftDatapack : IDatapack
    {
        public Byte PackVersion { get; set; }
        public String Description { get; set; }
    }
}
