using System;
using System.IO;

using Newtonsoft.Json;

namespace Helium.Commons.Configuration
{

	/// <summary>
	/// Part of the Helium Toolchain API. Abstract base class for JSON-serialized Language configurations
	/// </summary>
	public abstract class AbstractJsonLanguageConfiguration : AbstractLanguageConfiguration
	{
		public override void Serialize()
		{
			StreamWriter writer = new StreamWriter($"./{this.ConfigurationName}.json");
			writer.Write(JsonConvert.SerializeObject(this));
			writer.Close();
		}

		public override void Serialize(out String Serialized)
		{
			StreamWriter writer = new StreamWriter($"./{this.ConfigurationName}.json");
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
			StreamReader reader = new StreamReader($"./{this.ConfigurationName}.json");
			AbstractJsonLanguageConfiguration returnValue = JsonConvert.DeserializeObject<AbstractJsonLanguageConfiguration>(reader.ReadToEnd());
			reader.Close();
			return returnValue;
		}

		public override IConfiguration<String, String> Deserialize(String Filename)
		{
			StreamReader reader = new StreamReader($"./{Filename}");
			AbstractJsonLanguageConfiguration returnValue = JsonConvert.DeserializeObject<AbstractJsonLanguageConfiguration>(reader.ReadToEnd());
			reader.Close();
			return returnValue;
		}
	}
}
