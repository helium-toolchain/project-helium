namespace Helium.Nbt.Serialization;

using System;

using Helium.Nbt.Internal;

/// <summary>
/// Defines the two modes of <see cref="BinaryNbtReader"/> deserialization of <see cref="NbtInt32ArrayToken"/> and <see cref="NbtInt64ArrayToken"/>.
/// </summary>
public enum BinaryReaderEndianness
{
	/// <summary>
	/// Use the system default endianness. Slower initial parsing, but can be faster if you need to access a lot of
	/// array entries.
	/// </summary>
	Native,

	/// <summary>
	/// Force the binary reader to use <see cref="Int32BigEndian"/> and <see cref="Int64BigEndian"/> over <see cref="Int32"/>
	/// and <see cref="Int64"/>, respectively. Significantly faster initial parsing, but can be slower if you need to access
	/// a lot of array entries.
	/// </summary>
	BigEndian
}
