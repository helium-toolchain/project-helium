namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 64-bit integer tag
/// </summary>
[RequiresPreviewFeatures]
public struct Int64Tag : IValuedNbtToken<Int64>
{
	public static Byte Declarator => 0x04;

	public static Int32 Length => 8;

	public Int64 Value { get; init; }

	public Byte[] Name { get; init; }

	public Int64Tag(Byte[] name, Int64 value)
	{
		this.Name = name;
		this.Value = value;
	}
}
