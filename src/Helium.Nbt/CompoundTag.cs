namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT compound tag.
/// </summary>
[RequiresPreviewFeatures]
public class CompoundTag : NbtTag, ICollection
{
	public static new Byte Declarator => 0x0A;

	public List<INbtToken> Children { get; set; }

	public CompoundTag(Byte[] name, List<INbtToken> children)
	{
		this.Name = name;
		this.Children = children;
	}

	public Int32 Count => Children.Count;

	public Boolean IsSynchronized => false;

	public Object SyncRoot => null!;

	public void CopyTo(Array array, Int32 index)
	{
		Children.CopyTo((array as INbtToken[])!, index);
	}

	public IEnumerator GetEnumerator()
	{
		return Children.GetEnumerator();
	}
}
