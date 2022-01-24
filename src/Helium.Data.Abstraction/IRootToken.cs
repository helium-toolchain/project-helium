namespace Helium.Data.Abstraction;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents the root token of the data structure.
/// </summary>
[RequiresPreviewFeatures]
public interface IRootToken : ICompoundToken
{
	/// <summary>
	/// Notes which data structure this token was created as; to avoid using interfaces for obtaining types.
	/// </summary>
	public abstract static String DataFormat { get; }
}
