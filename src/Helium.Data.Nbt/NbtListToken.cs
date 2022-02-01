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
	internal List<IDataToken> children = new();

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
	/// The name of this list token. May never be null.
	/// </summary>
	public String Name { get; set; } = null!;

	/// <summary>
	/// The root token of this tree.
	/// </summary>
	public IRootToken? RootToken { get; set; }

	/// <summary>
	/// The immediate parent for this token.
	/// </summary>
	public IDataToken? ParentToken { get; set; }

	public Int32 Count => this.children.Count;

	public Boolean IsReadOnly => false;

	public void Add(IDataToken item)
	{
		this.children.Add(item);
	}

	public void AddChildToken(IDataToken token)
	{
		if(this.ListTypeDeclarator == token.RefDeclarator)
		{
			this.Add(token);
		}
		else
		{
			throw new ArgumentException($"Invalid token type: tried to add type {token.RefDeclarator} to a list of {this.ListTypeDeclarator}");
		}
	}

	public void Clear()
	{
		this.children.Clear();
	}

	public Boolean Contains(IDataToken item)
	{
		return this.children.Contains(item);
	}

	public void CopyTo(IDataToken[] array, Int32 arrayIndex)
	{
		this.children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<IDataToken> GetEnumerator()
	{
		return this.children.GetEnumerator();
	}

	public Int32 IndexOf(IDataToken item)
	{
		return this.children.IndexOf(item);
	}

	public void Insert(Int32 index, IDataToken item)
	{
		this.children.Insert(index, item);
	}

	public Boolean Remove(IDataToken item)
	{
		return this.children.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		this.children.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.children.GetEnumerator();
	}
}
