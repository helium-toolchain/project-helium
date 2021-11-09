namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

using Helium.Nbt.Internal;

/// <summary>
/// Represents a length-prefixed array of signed 64-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtInt64BigEndianArrayToken : IValuedComplexNbtToken<Int64BigEndian>, IList<Int64BigEndian>
{
	private readonly List<Int64BigEndian> elements;

	public Int64BigEndian this[Int32 index]
	{
		get => elements[index];
		set => elements[index] = value;
	}

	public static Byte Declarator => 0x0C;

	public NbtInt64BigEndianArrayToken(Byte[] name, Span<Int64BigEndian> values, IComplexNbtToken parent)
	{
		this.Name = name;
		this.elements = values.ToArray().ToList();
		this.Parent = parent;
	}

	public Int32 Count => elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; set; }

	public IComplexNbtToken Parent { get; set; }

	public void Add(Int64BigEndian item)
	{
		elements.Add(item);
	}

	public void Clear()
	{
		elements.Clear();
	}

	public Boolean Contains(Int64BigEndian item)
	{
		return elements.Contains(item);
	}

	public void CopyTo(Int64BigEndian[] array, Int32 arrayIndex)
	{
		elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int64BigEndian> GetEnumerator()
	{
		return elements.GetEnumerator();
	}

	public Int32 IndexOf(Int64BigEndian item)
	{
		return elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int64BigEndian item)
	{
		elements.Insert(index, item);
	}

	public Boolean Remove(Int64BigEndian item)
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

	public void AddChild(Int64BigEndian token)
	{
		this.Add(token);
	}
}
