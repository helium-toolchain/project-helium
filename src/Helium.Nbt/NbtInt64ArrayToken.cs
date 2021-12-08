namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of signed 64-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtInt64ArrayToken : IValuedComplexNbtToken<Int64>, IList<Int64>
{
	public List<Int64> Elements { get; set; }

	public Int64 this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x0C;

	public NbtInt64ArrayToken(Byte[] name, List<Int64> values)
	{
		this.Name = name;
		this.Elements = values;
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; set; }

	public INbtToken? Parent { get; set; }

	public void Add(Int64 item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(Int64 item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(Int64[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int64> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(Int64 item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int64 item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(Int64 item)
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

	public void AddChild(Int64 token)
	{
		this.Add(token);
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtInt64ArrayToken t = (NbtInt64ArrayToken)token;
		Span<Byte> buffer = stackalloc Byte[4], buffer2 = stackalloc Byte[8];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Count);

		stream.Write(buffer);

		foreach(Int64 i in t.Elements)
		{
			BinaryPrimitives.WriteInt64BigEndian(buffer2, i);

			stream.Write(buffer2);
		}
	}
}
