namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a length-prefixed array of any NBT token.
/// </summary>
/// <remarks>
/// It is prefixed with one byte signifying the token types it contains and a signed 32-bit integer signifying the length.
/// </remarks>
[RequiresPreviewFeatures]
public sealed class NbtListToken : 
	IValuedComplexNbtToken<INbtToken>, IList<INbtToken>, ITypelessList
{
	public List<INbtToken> Content { get; set; }

	public Int32 TargetLength { get; set; }

	public static Byte Declarator => 0x09;

	public NbtListToken(Byte[] name, List<INbtToken> content, Int32 targetLength, NbtTokenType tokenType)
	{
		this.Name = name;
		this.Content = content;
		this.TargetLength = targetLength;

		this.ListTokenType = tokenType;
	}

	public INbtToken this[Int32 index]
	{
		get => Content[index];
		set => Content[index] = value;
	}

	public Int32 Count => Content.Count;

	public Boolean IsReadOnly => false;

	public Byte[] Name { get; set; }

	public static Int32 Length => 0;

	public NbtTokenType ListTokenType { get; set; }

	public void Add(INbtToken item)
	{
		Content.Add(item);
	}

	public void Clear()
	{
		Content.Clear();
	}

	public Boolean Contains(INbtToken item)
	{
		return Content.Contains(item);
	}

	public void CopyTo(INbtToken[] array, Int32 arrayIndex)
	{
		Content.CopyTo(array, arrayIndex);
	}

	public IEnumerator<INbtToken> GetEnumerator()
	{
		return Content.GetEnumerator();
	}

	public Int32 IndexOf(INbtToken item)
	{
		return Content.IndexOf(item);
	}

	public void Insert(Int32 index, INbtToken item)
	{
		Content.Insert(index, item);
	}

	public Boolean Remove(INbtToken item)
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

	public void AddChild(INbtToken token)
	{
		throw new NotImplementedException();
	}

	[Obsolete("Lists should not be nested, therefore this method does not need support")]
	public static void WriteNameless(Stream stream, INbtToken token)
		=> throw new NotImplementedException("Lists should not be nested.");
}
