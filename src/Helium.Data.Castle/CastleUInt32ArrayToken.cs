namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleUUInt32ArrayToken : ICastleToken, IArrayToken<UInt32>
{
	internal List<UInt32> Children { get; set; }

	public UInt32 this[Int32 index]
	{
		get => this.Children[index];
		set => this.Children[index] = value;
	}

	public static Byte Declarator => 0x16;

	public UInt16 NameId { get; internal set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleUInt32ArrayToken {NameId} was not of type CastleRootToken");

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

	public void Add(UInt32 item)
	{
		this.Children.Add(item);
	}

	public void Clear()
	{
		this.Children.Clear();
	}

	public Boolean Contains(UInt32 item)
	{
		return this.Children.Contains(item);
	}

	public void CopyTo(UInt32[] array, Int32 arrayIndex)
	{
		this.Children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<UInt32> GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}

	public Int32 IndexOf(UInt32 item)
	{
		return this.Children.IndexOf(item);
	}

	public void Insert(Int32 index, UInt32 item)
	{
		this.Children.Insert(index, item);
	}

	public Boolean Remove(UInt32 item)
	{
		return this.Children.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		this.Children.RemoveAt(index);
	}

	public IDataToken ToNbtToken()
	{
		NbtInt32ArrayToken nbt = new()
		{
			Name = this.Name
		};

		nbt.SetChildren(MemoryMarshal.Cast<UInt32, Int32>(CollectionsMarshal.AsSpan(this.Children)));

		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
