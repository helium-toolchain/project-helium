﻿namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of signed 64-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public class Int64ArrayTag : NbtTag, IList<Int64>
{
	private readonly List<Int64> elements;

	public Int64 this[Int32 index]
	{
		get => elements[index];
		set => elements[index] = value;
	}

	public static new Byte Declarator => 0x0C;

	public Int64ArrayTag(Byte[] name, Span<Int64> values)
	{
		this.Name = name;
		this.elements = values.ToArray().ToList();
	}

	public Int32 Count => elements.Count;

	public Boolean IsReadOnly => false;

	public void Add(Int64 item)
	{
		elements.Add(item);
	}

	public void Clear()
	{
		elements.Clear();
	}

	public Boolean Contains(Int64 item)
	{
		return elements.Contains(item);
	}

	public void CopyTo(Int64[] array, Int32 arrayIndex)
	{
		elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int64> GetEnumerator()
	{
		return elements.GetEnumerator();
	}

	public Int32 IndexOf(Int64 item)
	{
		return elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int64 item)
	{
		elements.Insert(index, item);
	}

	public Boolean Remove(Int64 item)
	{
		return elements.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		elements.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return elements.GetEnumerator();
	}
}
