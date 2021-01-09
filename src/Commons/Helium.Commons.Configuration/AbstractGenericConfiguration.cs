using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Configuration
{
    /// <summary>
    /// Part of the Helium Toolchain API. Serves as base class for generic configurations.
    /// </summary>
    public abstract class AbstractGenericConfiguration : IConfiguration<String, Object>
    {
        public String ConfigurationName { get; init; }
        public String DataVersion { get; set; }
        public List<String> LoadedModifications { get; set; }
        public Dictionary<String, Object> Configuration { get; set; }

        public abstract IConfiguration<String, Object> Deserialize();
        public abstract IConfiguration<String, Object> Deserialize(String Filename);
        public abstract void Serialize();
        public abstract void Serialize(out String Serialized);
        public abstract void Serialize(String Filename);
        public abstract void Serialize(String Filename, out String Serialized);
    }
}
