namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of signed bytes.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtByteArrayToken : IValuedComplexNbtToken<SByte>, IList<SByte>
{
	public List<SByte> Elements { get; set; }

	public SByte this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x07;

	public NbtByteArrayToken(Byte[] name, Span<SByte> values)
	{
		this.Name = name;
		this.Elements = values.ToArray().ToList();
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; init; }

	public void Add(SByte item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(SByte item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(SByte[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<SByte> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(SByte item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, SByte item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(SByte item)
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

	public void AddChild(SByte token)
	{
		this.Add(token);
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtByteArrayToken t = (NbtByteArrayToken)token;
		Span<Byte> buffer = stackalloc Byte[4];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Count);

		stream.Write(buffer);
		stream.Write(MemoryMarshal.Cast<SByte, Byte>(CollectionsMarshal.AsSpan(t.Elements)));
	}
}
