namespace Helium.Data.Abstraction;

using System.Runtime.Versioning;

/// <summary>
/// Represents a token holding a singular, primitive value.
/// </summary>
/// <typeparam name="T">The primitive value represented by this token.</typeparam>
[RequiresPreviewFeatures]
public interface IValueToken<T> : IDataToken
{
	public T Value { get; }
}
