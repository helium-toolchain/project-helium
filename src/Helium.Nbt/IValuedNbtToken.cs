namespace Helium.Nbt;

using System.Runtime.Versioning;

[RequiresPreviewFeatures]
public interface IValuedNbtToken<TValue> : INbtToken
{
	/// <summary>
	/// Represents the value of this token. This is only valid for name-value tokens, not for compounds or lists.
	/// </summary>
	public TValue Value { get; }
}
