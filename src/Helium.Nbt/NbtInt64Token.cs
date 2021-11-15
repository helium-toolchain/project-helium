namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 64-bit integer token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtInt64Token : IValuedNbtToken<Int64>
{
	public static Byte Declarator => 0x04;

	public static Int32 Length => 8;

	public Int64 Value { get; init; }

	public Byte[] Name { get; init; }

	public NbtInt64Token(Byte[] name, Int64 value)
	{
		this.Name = name;
		this.Value = value;
	}
}
