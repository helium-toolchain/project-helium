namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 32-bit integer tag
/// </summary>
[RequiresPreviewFeatures]
public struct Int32Tag : IValuedNbtToken<Int32>
{
	public static Byte Declarator => 0x03;

	public static Int32 Length => 4;

	public Int32 Value { get; init; }

	public Byte[] Name { get; init; }

	public Int32Tag(Byte[] name, Int32 value)
	{
		this.Name = name;
		this.Value = value;
	}
}
