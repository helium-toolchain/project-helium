namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT compound token.
/// </summary>
[RequiresPreviewFeatures]
public sealed class NbtCompoundToken : IComplexNbtToken, ICollection
{
	public static Byte Declarator => 0x0A;

	public List<INbtToken> Children { get; set; }

	public NbtCompoundToken(Byte[] name, List<INbtToken> children, IComplexNbtToken parent)
	{
		this.Name = name;
		this.Children = children;
		this.Parent = parent;
	}

	public NbtCompoundToken()
	{
		Name = Array.Empty<Byte>();
		Children = new();
		this.Parent = null!;
	}

	public Int32 Count => Children.Count;

	public Boolean IsSynchronized => false;

	public Object SyncRoot => null!;

	public static Int32 Length => 0;

	public Byte[] Name { get; init; }

	public IComplexNbtToken Parent { get; set; }

	public void CopyTo(Array array, Int32 index)
	{
		Children.CopyTo((array as INbtToken[])!, index);
	}

	public IEnumerator GetEnumerator()
	{
		return Children.GetEnumerator();
	}

	public void AddChild(INbtToken token)
	{
		throw new NotImplementedException();
	}
}
