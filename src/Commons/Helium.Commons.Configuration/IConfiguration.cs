using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Configuration
{
    /// <summary>
    /// Part of the Helium Toolchain API. This interface defines all methods and fields a configuration needs to define.
    /// Serialization attributes like [JsonIgnore] or [JsonProperty] should be defined in the inheriting class(es)
    /// </summary>
    public interface IConfiguration<TConfigurationKey, TConfigurationValue>
		where TConfigurationKey : notnull
    {
        /// <summary>
        /// This should mostly be used for dynamic serialization and backups. Try to not store important data here...
        /// </summary>
        public String ConfigurationName { get; init; }

        /// <summary>
        /// Defines the exact executable version the configuration was last saved with. Required for datafixing.
        /// </summary>
        public String DataVersion { get; set; }

        /// <summary>
        /// Defines the assembly names of all loaded modifications during the last config save.
        /// This should never be ignored, particularly not if your modification adds or removes config keys.
        /// </summary>
        public List<String> LoadedModifications { get; set; }

        /// <summary>
        /// Fill this with your configuration values
        /// </summary>
        public Dictionary<TConfigurationKey, TConfigurationValue> Configuration { get; set; }

        /// <summary>
        /// Indexer to quickly access configuration values without having to use the dictionary.
        /// Depending on the defined error severity level, this might throw exceptions or return unexpected values
        /// </summary>
        /// <param name="key">The configuration key referenced - it is important the key actually exists</param>
        /// <returns>The requested configuration value, provided the key actually exists</returns>
        public TConfigurationValue this[TConfigurationKey key] {
            get {
                if(Configuration.ContainsKey(key))
                    return Configuration[key];
                else
#pragma warning disable CS8603
					return default;
#pragma warning restore CS8603
			}

            set {
                if(Configuration.ContainsKey(key))
                    Configuration[key] = value;
            }
        }

        /// <summary>
        /// Attempts to safely add a new configuration entry.
        /// </summary>
        /// <param name="Key">Configuration key to add</param>
        /// <param name="Value">Configuration value to add</param>
        /// <param name="Overwrite">Whether or not to overwrite a possibly existing configuration value</param>
        /// <returns>Whether the attempt was successful</returns>
        public Boolean? TryAddNewConfigurationEntry(TConfigurationKey Key, TConfigurationValue Value, Boolean Overwrite = false)
        {
            if(Configuration.ContainsKey(Key))
                if(Overwrite) {
                    Configuration[Key] = Value;
                    return true;
                }
                else
                    return false;

            return Configuration.TryAdd(Key, Value);
        }

        /// <summary>
        /// Add a single new configuration entry. This method can be chained.
        /// </summary>
        /// <param name="Key">Configuration key to add</param>
        /// <param name="Value">Configuration value to add</param>
        /// <param name="Overwrite">Whether or not to overwrite a possibly existing configuration value</param>
        public IConfiguration<TConfigurationKey, TConfigurationValue> AddNewConfigurationEntry(TConfigurationKey Key, 
            TConfigurationValue Value, Boolean Overwrite = false)
        {
            if(Configuration.ContainsKey(Key) && Overwrite)
                Configuration[Key] = Value;
            else if(Configuration.ContainsKey(Key) && !Overwrite)
                return this;
            else
                Configuration.Add(Key, Value);

            return this;
        }

        /// <summary>
        /// Attempts to safely add a list of keys and values to the existing configuration
        /// </summary>
        /// <param name="Keys">Configuration keys to add</param>
        /// <param name="Values">Configuration values to add</param>
        /// <param name="Overwrite">Whether or not to overwrite a possibly existing configuration value</param>
        /// <returns>Whether or not all entries were added successfully</returns>
        public Boolean? TryAddNewConfigurationEntries(IEnumerable<TConfigurationKey> Keys,
            IEnumerable<TConfigurationValue> Values, Boolean Overwrite = false)
        {
            if(Keys.Count() != Values.Count()) {
                if(Keys.Count() > Values.Count())
                    throw new ArgumentException("Cannot add empty keys to a configuration", nameof(Keys));
                else
                    throw new ArgumentException("Cannot add values without keys to a configuration", nameof(Values));
            }

            var KeyArray = Keys.ToImmutableArray();
            var ValueArray = Values.ToImmutableArray();

            try {
                for(Int32 i = 0; i < Keys.Count(); i++) {
                    if(Overwrite) {
                        if(Configuration.ContainsKey(KeyArray[i]))
                            Configuration[KeyArray[i]] = ValueArray[i];
                        else
                            Configuration.Add(KeyArray[i], ValueArray[i]);
                    } else if(Configuration.ContainsKey(KeyArray[i])) {
                        return false;
                    } else {
                        Configuration.Add(KeyArray[i], ValueArray[i]);
                    }
                }
            }
            catch {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a list of keys and values to the existing configuration. This method can be chained.
        /// </summary>
        /// <param name="Keys">Configuration keys to add</param>
        /// <param name="Values">Configuration values to add</param>
        /// <param name="Overwrite">Whether or not to overwrite possibly existing entries</param>
        public IConfiguration<TConfigurationKey, TConfigurationValue> AddNewConfigurationEntries(IEnumerable<TConfigurationKey> Keys, 
            IEnumerable<TConfigurationValue> Values, Boolean Overwrite = false)
        {
            if(Keys.Count() != Values.Count()) {
                if(Keys.Count() > Values.Count())
                    throw new ArgumentException("Cannot add empty keys to a configuration", nameof(Keys));
                else
                    throw new ArgumentException("Cannot add values without keys to a configuration", nameof(Values));
            }

            var KeyArray = Keys.ToImmutableArray();
            var ValueArray = Values.ToImmutableArray();

            for(Int32 i = 0; i < Keys.Count(); i++) {
                if(Overwrite) {
                    if(Configuration.ContainsKey(KeyArray[i]))
                        Configuration[KeyArray[i]] = ValueArray[i];
                    else
                        Configuration.Add(KeyArray[i], ValueArray[i]);
                } else if(Configuration.ContainsKey(KeyArray[i])) {
                    throw new ArgumentException("Cannot overwrite existing configuration entries, Overwrite mode is disabled", nameof(Keys));
                } else {
                    Configuration.Add(KeyArray[i], ValueArray[i]);
                }
            }

            return this;
        }

        /// <summary>
        /// Saves the configuration to file
        /// </summary>
        public void Serialize();

        /// <summary>
        /// Saves the configuration to file and returns the serialized string
        /// </summary>
        public void Serialize(out String Serialized);

        /// <summary>
        /// Saves the configuration to the specified file
        /// </summary>
        public void Serialize(String Filename);

        /// <summary>
        /// Saves the configuration to the specified file and returns the serialized string
        /// </summary>
        public void Serialize(String Filename, out String Serialized);

        /// <summary>
        /// Reads the configuration from a file
        /// </summary>
        public IConfiguration<TConfigurationKey, TConfigurationValue> Deserialize();

        /// <summary>
        /// Reads the configuration from the specified file
        /// </summary>
        public IConfiguration<TConfigurationKey, TConfigurationValue> Deserialize(String Filename);
    }
}
