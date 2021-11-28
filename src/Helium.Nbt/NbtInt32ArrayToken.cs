namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	public List<Int32> Elements { get; set; }

	public Int32 this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x0B;

	public NbtInt32ArrayToken(Byte[] name, List<Int32> values)
	{
		this.Name = name;
		this.Elements = values;
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; private set; }

	public INbtToken? Parent { get; set; }

	public void Add(Int32 item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(Int32 item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(Int32[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int32> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(Int32 item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int32 item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(Int32 item)
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

	public void AddChild(Int32 token)
	{
		this.Add(token);
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtInt32ArrayToken t = (NbtInt32ArrayToken)token;
		Span<Byte> buffer = stackalloc Byte[4];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Count);

		stream.Write(buffer);

		foreach(Int32 i in t.Elements)
		{
			BinaryPrimitives.WriteInt32BigEndian(buffer, i);

			stream.Write(buffer);
		}
	}
}

