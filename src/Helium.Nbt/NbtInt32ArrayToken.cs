namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of signed 32-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtInt32ArrayToken : IValuedComplexNbtToken<Int32>, IList<Int32>
{
	private readonly List<Int32> elements;

	public Int32 this[Int32 index]
	{
		get => elements[index];
		set => elements[index] = value;
	}

	public static Byte Declarator => 0x0B;

	public NbtInt32ArrayToken(Byte[] name, Span<Int32> values, IComplexNbtToken parent)
	{
		this.Name = name;
		this.elements = values.ToArray().ToList();
		this.Parent = parent;
	}

	public Int32 Count => elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; private set; }

	public IComplexNbtToken Parent { get; set; }

	public void Add(Int32 item)
	{
		elements.Add(item);
	}

	public void Clear()
	{
		elements.Clear();
	}

	public Boolean Contains(Int32 item)
	{
		return elements.Contains(item);
	}

	public void CopyTo(Int32[] array, Int32 arrayIndex)
	{
		elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int32> GetEnumerator()
	{
		return elements.GetEnumerator();
	}

	public Int32 IndexOf(Int32 item)
	{
		return elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int32 item)
	{
		elements.Insert(index, item);
	}

	public Boolean Remove(Int32 item)
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

	public void AddChild(Int32 token)
	{
		
	}
}

