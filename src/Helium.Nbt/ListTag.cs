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
public class ListTag<TPrimitive> : NbtTag, IList<TPrimitive>, ITypelessList
{
	public List<TPrimitive> Content { get; set; }
	public new Int32 Length { get; set; }

	public static new Byte Declarator => 0x09;

	public ListTag(Byte[] name, List<TPrimitive> content, Int32 length)
	{
		this.Name = name;
		this.Content = content;
		this.Length = length;
	}

	public TPrimitive this[Int32 index]
	{
		get => Content[index];
		set => Content[index] = value;
	}

	public Int32 Count => Content.Count;

	public Boolean IsReadOnly => false;

	public void Add(TPrimitive item)
	{
		Content.Add(item);
	}

	public void Clear()
	{
		Content.Clear();
	}

	public Boolean Contains(TPrimitive item)
	{
		return Content.Contains(item);
	}

	public void CopyTo(TPrimitive[] array, Int32 arrayIndex)
	{
		Content.CopyTo(array, arrayIndex);
	}

	public IEnumerator<TPrimitive> GetEnumerator()
	{
		return Content.GetEnumerator();
	}

	public Int32 IndexOf(TPrimitive item)
	{
		return Content.IndexOf(item);
	}

	public void Insert(Int32 index, TPrimitive item)
	{
		Content.Insert(index, item);
	}

	public Boolean Remove(TPrimitive item)
	{
		return Content.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		Content.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Content.GetEnumerator();
	}
}
