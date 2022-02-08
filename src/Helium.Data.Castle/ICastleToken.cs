namespace Helium.Data.Castle;

using System.Runtime.Versioning;

using Helium.Data.Abstraction;

/// <summary>
/// Base interface for Castle-type tokens.
/// </summary>
[RequiresPreviewFeatures]
public interface ICastleToken
{
	/// <summary>
	/// Returns a NBT representation of this token. This is only intended for rudimentary converters.
	/// </summary>
	/// <returns></returns>
	public IDataToken ToNbtToken();
}
