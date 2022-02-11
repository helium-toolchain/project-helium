namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleUInt16ArrayToken : ICastleToken, IArrayToken<UInt16>
{
	internal List<UInt16> Children { get; set; }

	public UInt16 this[Int32 index]
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

	public void Add(UInt16 item)
	{
		this.Children.Add(item);
	}

	public void Clear()
	{
		this.Children.Clear();
	}

	public Boolean Contains(UInt16 item)
	{
		return this.Children.Contains(item);
	}

	public void CopyTo(UInt16[] array, Int32 arrayIndex)
	{
		this.Children.CopyTo(array, arrayIndex);
	}

	public IEnumerator<UInt16> GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}

	public Int32 IndexOf(UInt16 item)
	{
		return this.Children.IndexOf(item);
	}

	public void Insert(Int32 index, UInt16 item)
	{
		this.Children.Insert(index, item);
	}

	public Boolean Remove(UInt16 item)
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

		if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported && this.Count >= 15)

		nbt.SetChildren(converted);

		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
