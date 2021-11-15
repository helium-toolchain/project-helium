﻿namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a big-endian IEEE-754 double-precision floating point token.
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtDoubleToken : IValuedNbtToken<Double>
{
	public static Byte Declarator => 0x06;

	public static Int32 Length => 8;

	public Double Value { get; init; }

	public Byte[] Name { get; init; }

	public NbtDoubleToken(Byte[] name, Double value)
	{
		this.Name = name;
		this.Value = value;
	}
}
