namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of signed bytes.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtByteArrayToken : IValuedComplexNbtToken<Byte>, IList<Byte>
{
	private readonly List<Byte> elements;

	public Byte this[Int32 index]
	{
		get => elements[index];
		set => elements[index] = value;
	}

	public static Byte Declarator => 0x07;

	public NbtByteArrayToken(Byte[] name, Span<Byte> values, IComplexNbtToken parent)
	{
		this.Name = name;
		this.elements = values.ToArray().ToList();
		this.Parent = parent;
	}

	public Int32 Count => elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; init; }

	public IComplexNbtToken Parent { get; set; }

	public void Add(Byte item)
	{
		elements.Add(item);
	}

	public void Clear()
	{
		elements.Clear();
	}

	public Boolean Contains(Byte item)
	{
		return elements.Contains(item);
	}

	public void CopyTo(Byte[] array, Int32 arrayIndex)
	{
		elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Byte> GetEnumerator()
	{
		return elements.GetEnumerator();
	}

	public Int32 IndexOf(Byte item)
	{
		return elements.IndexOf(item);
	}

	public void Insert(Int32 index, Byte item)
	{
		elements.Insert(index, item);
	}

	public Boolean Remove(Byte item)
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

	public void AddChild(Byte token)
	{
		this.Add(token);
	}
}
