namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT end token
/// </summary>
[RequiresPreviewFeatures]
public sealed class NbtEndToken : INbtToken
{
	public static Byte Declarator => 0x00;

	public static Int32 Length => 0;

	public Byte[] Name => null!;
}
