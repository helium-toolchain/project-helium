namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleSByteArrayToken : ICastleToken, IArrayToken<SByte>
{
	internal List<SByte> Children { get; set; }

	public SByte this[Int32 index]
	{
		get => this.Children[index];
		set => this.Children[index] = value;
	}

	public static Byte Declarator => 0x12;

	public UInt16 NameId { get; internal set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleSByteArrayToken {NameId} was not of type CastleRootToken");

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

	public void Add(SByte item)
	{
		this.Children.Add(item);
	}

	public void Clear()
	{
		this.Children.Clear();
	}

	public Boolean Contains(SByte item)
	{
		return this.Children.Contains(item);
	}

	public void CopyTo(SByte[] array, Int32 arrayIndex)
	{
		this.Children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<SByte> GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}

	public Int32 IndexOf(SByte item)
	{
		return this.Children.IndexOf(item);
	}

	public void Insert(Int32 index, SByte item)
	{
		this.Children.Insert(index, item);
	}

	public Boolean Remove(SByte item)
	{
		return this.Children.Remove(item);
	}

	public void RemoveAt(Int32 index)
	{
		this.Children.RemoveAt(index);
	}

	public IDataToken ToNbtToken()
	{
		NbtSByteArrayToken nbt = new()
		{
			Name = this.Name
		};

		nbt.children = this.Children;
		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
