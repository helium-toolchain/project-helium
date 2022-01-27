namespace Helium.Data.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public sealed record NbtSByteArrayToken : IArrayToken<SByte>
{
	private readonly List<SByte> children = new();

	public SByte this[Int32 index]
	{
		get => children[index];
		set => children[index] = value;
	}

	/// <summary>
	/// Stores the binary marker for this token type.
	/// </summary>
	public static Byte Declarator => 0x07;

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

	public void Add(SByte item)
	{
		this.children.Add(item);
	}

	public void Clear()
	{
		this.children.Clear();
	}

	public Boolean Contains(SByte item)
	{
		return this.children.Contains(item);
	}

	public void CopyTo(SByte[] array, Int32 arrayIndex)
	{
		this.children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<SByte> GetEnumerator()
	{
		return this.children.GetEnumerator();
	}

	public Int32 IndexOf(SByte item)
	{
		return this.children.IndexOf(item);
	}

	public void Insert(Int32 index, SByte item)
	{
		this.children.Insert(index, item);
	}

	public Boolean Remove(SByte item)
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

