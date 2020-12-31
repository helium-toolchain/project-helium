using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Configuration
{

    /// <summary>
    /// Part of the Helium Toolchain API. Abstract base class for JSON-serialized Language configurations
    /// </summary>
    public abstract class AbstractJsonLanguageConfiguration : AbstractLanguageConfiguration
    {
        public override void Serialize()
        {
            StreamWriter writer = new StreamWriter($"./{ConfigurationName}.json");
            writer.Write(JsonConvert.SerializeObject(this));
            writer.Close();
        }

        public override void Serialize(out String Serialized)
        {
            StreamWriter writer = new StreamWriter($"./{ConfigurationName}.json");
            writer.Write(Serialized = JsonConvert.SerializeObject(this));
            writer.Close();
        }

        public override void Serialize(String Filename)
        {
            StreamWriter writer = new StreamWriter($"./{Filename}");
            writer.Write(JsonConvert.SerializeObject(this));
            writer.Close();
        }

        public override void Serialize(String Filename, out String Serialized)
        {
            StreamWriter writer = new StreamWriter($"./{Filename}");
            writer.Write(Serialized = JsonConvert.SerializeObject(this));
            writer.Close();
        }

        public override IConfiguration<String, String> Deserialize()
        {
            StreamReader reader = new StreamReader($"./{ConfigurationName}.json");
            IConfiguration<String, String> returnValue = JsonConvert.DeserializeObject<IConfiguration<String, String>>(reader.ReadToEnd());
            reader.Close();
            return returnValue;
        }

        public override IConfiguration<String, String> Deserialize(String Filename)
        {
            StreamReader reader = new StreamReader($"./{Filename}");
            IConfiguration<String, String> returnValue = JsonConvert.DeserializeObject<IConfiguration<String, String>>(reader.ReadToEnd());
            reader.Close();
            return returnValue;
        }
    }
}
