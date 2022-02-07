namespace Helium.Data.Abstraction;

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a token containing one or more child tokens of unspecified type.
/// </summary>
[RequiresPreviewFeatures]
public interface ICompoundToken : IDataToken, IDictionary<String, IDataToken>
{
	/// <summary>
	/// A list of child tokens represented by this compound.
	/// </summary>
	public IEnumerable<IDataToken> Children { get; }

	/// <summary>
	/// Adds a child token to this compound.
	/// </summary>
	/// <param name="token">The child token to be added.</param>
	public void AddChildToken(IDataToken token);
}
