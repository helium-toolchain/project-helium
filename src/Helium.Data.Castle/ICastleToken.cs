namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

/// <summary>
/// Base interface for Castle-type tokens.
/// </summary>
[RequiresPreviewFeatures]
public interface ICastleToken : IDataToken
{
	/// <summary>
	/// Returns a NBT representation of this token. This is only intended for rudimentary converters.
	/// </summary>
	/// <returns></returns>
	public IDataToken ToNbtToken();

	/// <summary>
	/// Stores the ID of this tokens name in the root array.
	/// </summary>
	public UInt16 NameId { get; }
}
