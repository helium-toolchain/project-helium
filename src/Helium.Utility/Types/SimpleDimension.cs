using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Utility.Types
{
    public record SimpleDimension
    {
        public Int32 Id { get; init; }
        public String FolderPath { get; init; }
        public String Name { get; init; }
    }
}
