namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of any NBT token.
/// </summary>
/// <remarks>
/// It is prefixed with one byte signifying the token types it contains and a signed 32-bit integer signifying the length.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtListToken<TPrimitive> : 
	IValuedComplexNbtToken<TPrimitive>, IList<TPrimitive>, ITypelessList
{
	public List<TPrimitive> Content { get; set; }

	public Int32 TargetLength { get; set; }

	public static Byte Declarator => 0x09;

	public NbtListToken(Byte[] name, List<TPrimitive> content, IComplexNbtToken parent, Int32 targetLength)
	{
		this.Name = name;
		this.Content = content;
		this.Parent = parent;
		this.TargetLength = targetLength;

		switch(default(TPrimitive))
		{
			case NbtBigEndianInt32ArrayToken:
			case NbtInt32ArrayToken:
				this.ListTokenType = NbtTokenType.IntArray;
				break;
			case NbtBigEndianInt64ArrayToken:
			case NbtInt64ArrayToken:
				this.ListTokenType = NbtTokenType.LongArray;
				break;
			case NbtByteArrayToken:
				this.ListTokenType = NbtTokenType.ByteArray;
				break;
			case Byte:
				this.ListTokenType = NbtTokenType.Byte;
				break;
			case NbtCompoundToken:
				this.ListTokenType = NbtTokenType.Compound;
				break;
			case Double:
				this.ListTokenType = NbtTokenType.Double;
				break;
			case Int16:
				this.ListTokenType = NbtTokenType.Short;
				break;
			case Int32:
				this.ListTokenType = NbtTokenType.Int;
				break;
			case Int64:
				this.ListTokenType = NbtTokenType.Long;
				break;
			case ITypelessList:
				this.ListTokenType = NbtTokenType.List;
				break;
			case Single:
				this.ListTokenType = NbtTokenType.Float;
				break;
			case NbtStringToken:
				this.ListTokenType = NbtTokenType.String;
				break;
			case NbtEndToken:
				this.ListTokenType = NbtTokenType.End;
				break;
		}
	}

	public TPrimitive this[Int32 index]
	{
		get => Content[index];
		set => Content[index] = value;
	}

	public Int32 Count => Content.Count;

	public Boolean IsReadOnly => false;

	public Byte[] Name { get; set; }

	public IComplexNbtToken Parent { get; set; }

	public static Int32 Length => 0;

	public NbtTokenType ListTokenType { get; set; }

	public void Add(TPrimitive item)
	{
		Content.Add(item);
	}

	public void Clear()
	{
		Content.Clear();
	}

	public Boolean Contains(TPrimitive item)
	{
		return Content.Contains(item);
	}

	public void CopyTo(TPrimitive[] array, Int32 arrayIndex)
	{
		Content.CopyTo(array, arrayIndex);
	}

	public IEnumerator<TPrimitive> GetEnumerator()
	{
		return Content.GetEnumerator();
	}

	public Int32 IndexOf(TPrimitive item)
	{
		return Content.IndexOf(item);
	}

	public void Insert(Int32 index, TPrimitive item)
	{
		Content.Insert(index, item);
	}

	public Boolean Remove(TPrimitive item)
	{
		return Content.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		Content.RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Content.GetEnumerator();
	}

	public void AddChild(TPrimitive token)
	{
		throw new NotImplementedException();
	}
}
