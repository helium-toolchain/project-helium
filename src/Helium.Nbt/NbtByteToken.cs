namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT Byte Token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtByteToken : IValuedNbtToken<Byte>
{
	public static Byte Declarator => 0x01;

	public static Int32 Length => 1;

	public Byte[] Name { get; init; }

	public Byte Value { get; init; }

	public NbtByteToken(Byte[] name, Byte value)
	{
		this.Name = name;
		this.Value = value;
	}
}
