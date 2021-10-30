namespace Helium.Nbt;

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents any NBT tag with children.
/// </summary>
[RequiresPreviewFeatures]
public abstract class NbtTag : INbtToken
{
	public static Byte Declarator => throw new NotImplementedException();

	public static Int32 Length => 0;

	public virtual List<INbtToken> Children { get; } = null!;

	public virtual Byte[] Name => null!;
}
