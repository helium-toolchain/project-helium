namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	public List<Byte> Elements { get; set; }

	public Byte this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x07;

	public NbtByteArrayToken(Byte[] name, Span<Byte> values)
	{
		this.Name = name;
		this.Elements = values.ToArray().ToList();
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; init; }

	public void Add(Byte item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(Byte item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(Byte[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Byte> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(Byte item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, Byte item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(Byte item)
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

	public void AddChild(Byte token)
	{
		this.Add(token);
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtByteArrayToken t = (NbtByteArrayToken)token;
		Span<Byte> buffer = stackalloc Byte[4];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Count);

		stream.Write(buffer);
		stream.Write(t.Elements.ToArray());
	}
}
