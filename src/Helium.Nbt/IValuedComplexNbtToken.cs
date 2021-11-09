namespace Helium.Nbt;

using System.Runtime.Versioning;

/// <summary>
/// Serves as a base type for all complex NBT tokens with a specified enumeration type.
/// </summary>
[RequiresPreviewFeatures]
public interface IValuedComplexNbtToken<TValueType> : IComplexNbtToken
{
	/// <summary>
	/// Adds a child token to the current complex NbtToken
	/// </summary>
	/// <param name="token"></param>
	public void AddChild(TValueType token);
}
