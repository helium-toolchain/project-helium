namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

using Helium.Nbt.Internal;

/// <summary>
/// Represents a length-prefixed array of signed 32-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtBigEndianInt32ArrayToken : IValuedComplexNbtToken<Int32BigEndian>, IList<Int32BigEndian>
{
	public List<Int32BigEndian> Elements { get; set; }

	public Int32BigEndian this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x0B;

	public NbtBigEndianInt32ArrayToken(Byte[] name, Span<Int32BigEndian> values)
	{
		this.Name = name;
		this.Elements = values.ToArray().ToList();
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; private set; }

	public void Add(Int32BigEndian item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(Int32BigEndian item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(Int32BigEndian[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int32BigEndian> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(Int32BigEndian item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int32BigEndian item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(Int32BigEndian item)
	{
		return Elements.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		Elements.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public void AddChild(Int32BigEndian token)
	{
		this.Add(token);
	}
}

