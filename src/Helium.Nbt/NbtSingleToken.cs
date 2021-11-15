﻿namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a big-endian IEEE-754 single-precision floating point token.
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtSingleToken : IValuedNbtToken<Single>
{
	public static Byte Declarator => 0x05;

	public static Int32 Length => 4;

	public Single Value { get; init; }

	public Byte[] Name { get; init; }

	public NbtSingleToken(Byte[] name, Single value)
	{
		this.Name = name;
		this.Value = value;
	}
}