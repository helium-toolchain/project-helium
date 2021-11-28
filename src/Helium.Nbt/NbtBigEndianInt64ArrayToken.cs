﻿namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Helium.Nbt.Internal;

/// <summary>
/// Represents a length-prefixed array of signed 64-bit integers.
/// </summary>
/// <remarks>
/// The prefix is a signed 32-bit integer.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtBigEndianInt64ArrayToken : IValuedComplexNbtToken<Int64BigEndian>, IList<Int64BigEndian>
{
	public List<Int64BigEndian> Elements { get; set; }

	public Int64BigEndian this[Int32 index]
	{
		get => Elements[index];
		set => Elements[index] = value;
	}

	public static Byte Declarator => 0x0C;

	public NbtBigEndianInt64ArrayToken(Byte[] name, Span<Int64BigEndian> values)
	{
		this.Name = name;
		this.Elements = values.ToArray().ToList();
	}

	public Int32 Count => Elements.Count;

	public Boolean IsReadOnly => false;

	public static Int32 Length => 0;

	public Byte[] Name { get; set; }

	public INbtToken? Parent { get; set; }

	public void Add(Int64BigEndian item)
	{
		Elements.Add(item);
	}

	public void Clear()
	{
		Elements.Clear();
	}

	public Boolean Contains(Int64BigEndian item)
	{
		return Elements.Contains(item);
	}

	public void CopyTo(Int64BigEndian[] array, Int32 arrayIndex)
	{
		Elements.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int64BigEndian> GetEnumerator()
	{
		return Elements.GetEnumerator();
	}

	public Int32 IndexOf(Int64BigEndian item)
	{
		return Elements.IndexOf(item);
	}

	public void Insert(Int32 index, Int64BigEndian item)
	{
		Elements.Insert(index, item);
	}

	public Boolean Remove(Int64BigEndian item)
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

	public void AddChild(Int64BigEndian token)
	{
		this.Add(token);
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtBigEndianInt64ArrayToken t = (NbtBigEndianInt64ArrayToken)token;
		Span<Byte> buffer = stackalloc Byte[4];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Count);

		stream.Write(buffer);
		stream.Write(MemoryMarshal.Cast<Int64BigEndian, Byte>(CollectionsMarshal.AsSpan(t.Elements)));
	}
}
