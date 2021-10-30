namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 16-bit integer tag
/// </summary>
[RequiresPreviewFeatures]
public struct Int16Tag : IValuedNbtToken<Int16>
{
	public static Byte Declarator => 0x02;

	public static Int32 Length => 2;

	public Int16 Value { get; init; }

	public Byte[] Name { get; init; }

	public Int16Tag(Byte[] name, Int16 value)
	{
		this.Name = name;
		this.Value = value;
	}
}
