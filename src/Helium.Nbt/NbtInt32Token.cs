namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 32-bit integer token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtInt32Token : IValuedNbtToken<Int32>
{
	public static Byte Declarator => 0x03;

	public static Int32 Length => 4;

	public Int32 Value { get; init; }

	public Byte[] Name { get; init; }

	public IComplexNbtToken Parent { get; set; }

	public NbtInt32Token(Byte[] name, Int32 value, IComplexNbtToken parent)
	{
		this.Name = name;
		this.Value = value;
		this.Parent = parent;
	}
}
