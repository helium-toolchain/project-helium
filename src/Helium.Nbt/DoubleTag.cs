namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a big-endian IEEE-754 double-precision floating point tag.
/// </summary>
[RequiresPreviewFeatures]
public struct DoubleTag : IValuedNbtToken<Double>
{
	public static Byte Declarator => 0x06;

	public static Int32 Length => 8;

	public Double Value { get; init; }

	public Byte[] Name { get; init; }

	public DoubleTag(Byte[] name, Double value)
	{
		this.Name = name;
		this.Value = value;
	}
}
