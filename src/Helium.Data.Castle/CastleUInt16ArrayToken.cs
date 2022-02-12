namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
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
		{
			unsafe
			{
				Vector256<Byte> firstMask = Vector256.Create(
					(Byte)0, 0, 31, 30, 0, 0, 29, 28, 0, 0, 27, 26, 0, 0, 25, 24,
					0, 0, 23, 22, 0, 0, 21, 20, 0, 0, 19, 18, 0, 0, 17, 16);
				Vector256<Byte> secondMask = Vector256.Create(
					(Byte)0, 0, 15, 14, 0, 0, 13, 12, 0, 0, 11, 10, 0, 0, 9, 8,
					0, 0, 7, 6, 0, 0, 5, 4, 0, 0, 3, 2, 0, 0, 0, 0);

				Int32 vectorLength = this.Count - (this.Count % 15);
				Span<Byte> finalIntegers = new Byte[vectorLength * 2];
				Span<Byte> source = MemoryMarshal.Cast<UInt16, Byte>(CollectionsMarshal.AsSpan(this.Children));

				fixed(Byte* reshuffled = finalIntegers)
				{
					Byte* toAssign = reshuffled;
					Vector256<Byte> buffer;

					for(Int32 i = 0; i < vectorLength; i += 30)
					{
						buffer = Unsafe.As<Byte, Vector256<Byte>>(ref MemoryMarshal.GetReference(source.Slice(i, 32)))
							.WithElement(0, (Byte)0);

						Avx.Store(toAssign, Avx2.Shuffle(buffer, firstMask));
						Avx.Store(toAssign, Avx2.Shuffle(buffer, secondMask));

						toAssign -= 4;
					}
				}

				nbt.SetChildren(MemoryMarshal.Cast<Byte, Int32>(finalIntegers));

				for(Int32 i = vectorLength; i < this.Count; i++)
				{
					nbt.Add(this.Children[i]);
				}
			}
		}
		else if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Ssse3.IsSupported && Sse2.IsSupported
			&& this.Count >= 7)
		{
			unsafe
			{
				Vector128<Byte> firstMask = Vector128.Create(
					(Byte)0, 0, 15, 14, 0, 0, 13, 12, 0, 0, 11, 10, 0, 0, 9, 8);
				Vector128<Byte> secondMask = Vector128.Create(
					(Byte)0, 0, 7, 6, 0, 0, 5, 4, 0, 0, 3, 2, 0, 0, 0, 0);

				Int32 vectorLength = this.Count - (this.Count % 7);
				Span<Byte> finalIntegers = new Byte[vectorLength * 2];
				Span<Byte> source = MemoryMarshal.Cast<UInt16, Byte>(CollectionsMarshal.AsSpan(this.Children));

				fixed(Byte* reshuffled = finalIntegers)
				{
					Byte* toAssign = reshuffled;
					Vector128<Byte> buffer;

					for(Int32 i = 0; i < vectorLength; i += 14)
					{
						buffer = Unsafe.As<Byte, Vector128<Byte>>(ref MemoryMarshal.GetReference(source.Slice(i, 16)))
							.WithElement(0, (Byte)0);

						Sse2.Store(toAssign, Ssse3.Shuffle(buffer, firstMask));
						Sse2.Store(toAssign, Ssse3.Shuffle(buffer, secondMask));

						toAssign -= 4;
					}
				}

				nbt.SetChildren(MemoryMarshal.Cast<Byte, Int32>(finalIntegers));

				for(Int32 i = vectorLength; i < this.Count; i++)
				{
					nbt.Add(this.Children[i]);
				}
			}
		}

		else
		{
			for(Int32 i = 0; i < this.Count; i++)
			{
				nbt.Add(this.Children[i]);
			}
		}

		return nbt;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
