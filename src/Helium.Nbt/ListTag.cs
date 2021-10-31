namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of any NBT tag.
/// </summary>
/// <remarks>
/// It is prefixed with one byte signifying the tag types it contains and a signed 32-bit integer signifying the length.
/// </remarks>
[RequiresPreviewFeatures]
public class ListTag<TPrimitive> : NbtTag, IList<TPrimitive>
{
	private readonly List<TPrimitive> content;

	public static new Byte Declarator => 0x09;

	public ListTag(Byte[] name, List<TPrimitive> content)
	{
		this.Name = name;
		this.content = content;
	}

	public TPrimitive this[Int32 index]
	{
		get => content[index];
		set => content[index] = value;
	}

	public Int32 Count => content.Count;

	public Boolean IsReadOnly => false;

	public void Add(TPrimitive item)
	{
		content.Add(item);
	}

	public void Clear()
	{
		content.Clear();
	}

	public Boolean Contains(TPrimitive item)
	{
		return content.Contains(item);
	}

	public void CopyTo(TPrimitive[] array, Int32 arrayIndex)
	{
		content.CopyTo(array, arrayIndex);
	}

	public IEnumerator<TPrimitive> GetEnumerator()
	{
		return content.GetEnumerator();
	}

	public Int32 IndexOf(TPrimitive item)
	{
		return content.IndexOf(item);
	}

	public void Insert(Int32 index, TPrimitive item)
	{
		content.Insert(index, item);
	}

	public Boolean Remove(TPrimitive item)
	{
		return content.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		content.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return content.GetEnumerator();
	}
}
