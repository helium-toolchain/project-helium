namespace Helium.Data.Abstraction;

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a token serving as a list of other, nameless tokens of consistent type.
/// </summary>
[RequiresPreviewFeatures]
public interface IListToken : IDataToken, IList<IDataToken>
{
	/// <summary>
	/// Stores the type of all tokens in this list.
	/// </summary>
	public Byte ListTypeDeclarator { get; }

	/// <summary>
	/// Adds a child token <b>of the same type as the existing list</b> to the list.
	/// </summary>
	/// <param name="token">The child token to be added. This must match the existing list type.</param>
	/// <exception cref="ArgumentException">Thrown if the types do not match correctly and cannot be converted implicitly or explicitly.</exception>
	public void AddChildToken(IDataToken token);
}
