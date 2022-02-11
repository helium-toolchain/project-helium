namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;
using Helium.Data.Nbt.Serialization;

[RequiresPreviewFeatures]
public record struct CastleInt16ArrayToken : ICastleToken, IArrayToken<Int16>
{
	internal List<Int16> Children { get; set; }

	public Int16 this[Int32 index]
	{
		get => this.Children[index];
		set => this.Children[index] = value;
	}

	public static Byte Declarator => 0x11;

	public UInt16 NameId { get; internal set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleByteArrayToken {NameId} was not of type CastleRootToken");

			if(!root.TokenNames.Contains(value))
			{
				root.TokenNames.Add(value);
			}

			this.NameId = (UInt16)root.TokenNames.IndexOf(value);
		}
	}

	public IRootToken? RootToken { get; set; }

	public IDataToken? ParentToken { get; set; }

	public Int32 Count => this.Children.Count;

	public Boolean IsReadOnly => false;

	public void Add(Int16 item)
	{
		this.Children.Add(item);
	}

	public void Clear()
	{
		this.Children.Clear();
	}

	public Boolean Contains(Int16 item)
	{
		return this.Children.Contains(item);
	}

	public void CopyTo(Int16[] array, Int32 arrayIndex)
	{
		this.Children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Int16> GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}

	public Int32 IndexOf(Int16 item)
	{
		return this.Children.IndexOf(item);
	}

	public void Insert(Int32 index, Int16 item)
	{
		this.Children.Insert(index, item);
	}

	public Boolean Remove(Int16 item)
	{
		return this.Children.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		this.Children.RemoveAt(index);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public IDataToken ToNbtToken()
	{
		NbtInt32ArrayToken nbt = new()
		{
			Name = this.Name
		};

		Span<Int32> converted = new Int32[this.Count];

		for(Int32 i = 0; i < this.Count; i++)
		{
			converted[i] = (this.Children[i] & 0x8000) << 16 & this.Children[i] & 0x7FFF;
		}

		nbt.SetChildren(converted);

		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
