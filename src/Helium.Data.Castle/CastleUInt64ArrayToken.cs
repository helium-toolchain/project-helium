namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleUInt64ArrayToken : ICastleToken, IArrayToken<UInt64>
{
	internal List<UInt64> Children { get; set; }

	public UInt64 this[Int32 index]
	{
		get => this.Children[index];
		set => this.Children[index] = value;
	}

	public static Byte Declarator => 0x18;

	public UInt16 NameId { get; internal set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleUInt64ArrayToken {NameId} was not of type CastleRootToken");

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

	public void Add(UInt64 item)
	{
		this.Children.Add(item);
	}

	public void Clear()
	{
		this.Children.Clear();
	}

	public Boolean Contains(UInt64 item)
	{
		return this.Children.Contains(item);
	}

	public void CopyTo(UInt64[] array, Int32 arrayIndex)
	{
		this.Children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<UInt64> GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}

	public Int32 IndexOf(UInt64 item)
	{
		return this.Children.IndexOf(item);
	}

	public void Insert(Int32 index, UInt64 item)
	{
		this.Children.Insert(index, item);
	}

	public Boolean Remove(UInt64 item)
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

		nbt.SetChildren(MemoryMarshal.Cast<UInt64, Int32>(CollectionsMarshal.AsSpan(this.Children)));

		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
