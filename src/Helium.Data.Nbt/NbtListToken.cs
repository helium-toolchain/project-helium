namespace Helium.Data.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

/// <summary>
/// Represents a NBT List token.
/// </summary>
[RequiresPreviewFeatures]
public sealed record NbtListToken : IListToken
{
	private readonly List<IDataToken> children = new();

	public IDataToken this[Int32 index]
	{
		get => children[index];
		set => children[index] = value;
	}

	/// <summary>
	/// Stores the binary marker for this token type.
	/// </summary>
	public static Byte Declarator => 0x09;

	/// <summary>
	/// Provides an instance access field for this token type.
	/// </summary>
	public Byte RefDeclarator => Declarator;

	/// <summary>
	/// Stores which token type is tolerated as child for this list.
	/// </summary>
	public Byte ListTypeDeclarator { get; init; }

	/// <summary>
	/// The name of this list tag. May never be null.
	/// </summary>
	public String Name { get; set; } = null!;

	/// <summary>
	/// The root token of this tree.
	/// </summary>
	public IRootToken? RootToken { get; }

	/// <summary>
	/// The immediate parent for this token.
	/// </summary>
	public IDataToken? ParentToken { get; }

	public Int32 Count { get; }

	public Boolean IsReadOnly => false;

	public void Add(IDataToken item)
	{
		this.children.Add(item);
	}

	public void AddChildToken(IDataToken token)
	{
		
	}

	public void Clear()
	{
		throw new System.NotImplementedException();
	}

	public System.Boolean Contains(IDataToken item)
	{
		throw new System.NotImplementedException();
	}

	public void CopyTo(IDataToken[] array, System.Int32 arrayIndex)
	{
		throw new System.NotImplementedException();
	}

	public IEnumerator<IDataToken> GetEnumerator()
	{
		throw new System.NotImplementedException();
	}

	public System.Int32 IndexOf(IDataToken item)
	{
		throw new System.NotImplementedException();
	}

	public void Insert(System.Int32 index, IDataToken item)
	{
		throw new System.NotImplementedException();
	}

	public System.Boolean Remove(IDataToken item)
	{
		throw new System.NotImplementedException();
	}

	public void RemoveAt(System.Int32 index)
	{
		throw new System.NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new System.NotImplementedException();
	}
}
