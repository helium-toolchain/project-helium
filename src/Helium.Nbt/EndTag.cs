namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT end tag
/// </summary>
[RequiresPreviewFeatures]
public class EndTag : INbtToken
{
	public static Byte Declarator => 0x00;

	public static Int32 Length => 0;

	public Byte[] Name => null!;
}
