namespace Helium.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT compound token.
/// </summary>
[RequiresPreviewFeatures]
public sealed class NbtCompoundToken : IComplexNbtToken, ICollection
{
	public static Byte Declarator => 0x0A;

	public List<INbtToken> Children { get; set; }

	public NbtCompoundToken(Byte[] name, List<INbtToken> children)
	{
		this.Name = name;
		this.Children = children;
	}

	public NbtCompoundToken()
	{
		Name = Array.Empty<Byte>();
		Children = new();
	}

	public Int32 Count => Children.Count;

	public Boolean IsSynchronized => false;

	public Object SyncRoot => null!;

	public static Int32 Length => 0;

	public Byte[] Name { get; init; }

	public void CopyTo(Array array, Int32 index)
	{
		Children.CopyTo((array as INbtToken[])!, index);
	}

	public IEnumerator GetEnumerator()
	{
		return Children.GetEnumerator();
	}

	public void AddChild(INbtToken token)
	{
		throw new NotImplementedException();
	}

	[Obsolete("Not in use. Use BinaryNbtWriter#WriteCompoundToken(NbtCompoundToken, Boolean) instead.")]
	public static void WriteNameless(Stream stream, INbtToken token)
		=> throw new NotImplementedException("Use BinaryNbtWriter#WriteCompoundToken(NbtCompoundToken, Boolean) instead");
}
