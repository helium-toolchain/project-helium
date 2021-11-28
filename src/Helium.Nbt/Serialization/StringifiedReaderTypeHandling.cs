namespace Helium.Nbt.Serialization;

/// <summary>
/// Specifies how <see cref="StringifiedNbtReader"/> should handle datatypes.
/// </summary>
public enum StringifiedReaderTypeHandling
{
	/// <summary>
	/// Strict type handling. Wherever a type is unspecified, throw an error.
	/// This is consistent with mojangs behaviour.
	/// </summary>
	Strict,

	/// <summary>
	/// Allow inferring types where unspecified.
	/// </summary>
	Infer
}
