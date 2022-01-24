namespace Helium.Data.Abstraction;

using System;
using System.Runtime.Versioning;

/// <summary>
/// Represents the most basic serializable token.
/// </summary>
[RequiresPreviewFeatures]
public interface IDataToken
{
	/// <summary>
	/// Declarator for this token type in binary data.
	/// </summary>
	public abstract static Byte Declarator { get; }

	/// <summary>
	/// Stores the name of this token instance.
	/// </summary>
	public String Name { get; }

	/// <summary>
	/// Stores the root token for this token.
	/// </summary>
	public IRootToken? RootToken { get; }

	/// <summary>
	/// Stores the immediate parent token for this token.
	/// </summary>
	public IDataToken? ParentToken { get; }
}
