namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents any self-contained NBT token.
/// </summary>
[RequiresPreviewFeatures]
public interface INbtToken
{
	/// <summary>
	/// Gets the byte that declares this token type.
	/// </summary>
	public abstract static Byte Declarator { get; }

	/// <summary>
	/// Gets the binary-encoded length of this token type. 0 indicates a variable-length token.
	/// </summary>
	public abstract static Int32 Length { get; }

	/// <summary>
	/// Name of this Named Binary Data Token.
	/// </summary>
	public Byte[] Name { get; }
}
