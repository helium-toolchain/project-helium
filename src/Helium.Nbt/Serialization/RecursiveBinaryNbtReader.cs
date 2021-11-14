namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Helium.Nbt.Exceptions;
using Helium.Nbt.Internal;

[RequiresPreviewFeatures]
public class RecursiveBinaryNbtReader
{
	public NbtTokenType CurrentToken { get; set; }

	public Stream DataStream { get; set; }

	public RecursiveBinaryNbtReader(Stream input)
	{
		this.DataStream = input;
	}

	public NbtTokenType ReadTokenType()
	{
		Byte type = (Byte)this.DataStream.ReadByte();

		if(type > (Byte)NbtTokenType.LongArray)
		{
			throw new MalformedNbtException($"Invalid token type found: {type}");
		}

		return (NbtTokenType)type;
	}

	public INbtToken ReadCurrentToken(NbtTokenType tokenType, Boolean readName = true)
	{
		Byte[] name = Array.Empty<Byte>();

		if(readName)
		{
			name = this.ReadName();
		}

		return tokenType switch
		{
			NbtTokenType.End => new NbtEndToken(),
			NbtTokenType.Byte => new NbtByteToken(name, this.ReadByte(), null),
			NbtTokenType.Short => new NbtInt16Token(name, this.ReadInt16(), null),
			NbtTokenType.Int => new NbtInt32Token(name, this.ReadInt32(), null),
			NbtTokenType.Long => new NbtInt64Token(name, this.ReadInt64(), null),
			NbtTokenType.Float => new NbtSingleToken(name, this.ReadSingle(), null),
			NbtTokenType.Double => new NbtDoubleToken(name, this.ReadDouble(), null)
		};
	}

	public Byte[] ReadName()
	{
		Int16 length = this.ReadInt16();
		Span<Byte> buffer = stackalloc Byte[length];

		this.DataStream.Read(buffer);

		return buffer.ToArray();
	}

	#region read primitives

	public Byte ReadByte() => (Byte)this.DataStream.ReadByte();

	public Int16 ReadInt16()
	{
		Span<Byte> buffer = stackalloc Byte[2];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt16BigEndian(buffer);
	}

	public Int32 ReadInt32()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt32BigEndian(buffer);
	}

	public Int64 ReadInt64()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt64BigEndian(buffer);
	}

	public Single ReadSingle()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadSingleBigEndian(buffer);
	}

	public Double ReadDouble()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadDoubleBigEndian(buffer);
	}

	#endregion

	#region read complex types

	public NbtByteArrayToken ReadByteArray(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length];

		this.DataStream.Read(buffer);

		return new NbtByteArrayToken(name, buffer, null);
	}

	public NbtInt32ArrayToken ReadInt32Array(Byte[] name)
	{
		Int32 length = this.ReadInt32();
		List<Int32> array = new();

		for(Int32 i = 0; i < length; i++)
		{
			array.Add(this.ReadInt32());
		}

		return new NbtInt32ArrayToken(name, array, null);
	}

	public NbtBigEndianInt32ArrayToken ReadInt32ArrayBigEndian(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length * 4];

		this.DataStream.Read(buffer);

		return new NbtBigEndianInt32ArrayToken(name, MemoryMarshal.Cast<Byte, Int32BigEndian>(buffer), null);
	}

	public NbtInt64ArrayToken ReadInt64Array(Byte[] name)
	{
		Int32 length = this.ReadInt32();
		List<Int64> array = new();

		for(Int32 i = 0; i < length; i++)
		{
			array.Add(this.ReadInt64());
		}

		return new NbtInt64ArrayToken(name, array, null);
	}

	public NbtBigEndianInt64ArrayToken ReadInt64ArrayBigEndian(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length * 8];

		this.DataStream.Read(buffer);

		return new NbtBigEndianInt64ArrayToken(name, MemoryMarshal.Cast<Byte, Int64BigEndian>(buffer), null);
	}

	public NbtCompoundToken ReadCompoundToken(Byte[] name)
	{
		NbtCompoundToken compound = new(name, new(), null);

		NbtTokenType type;

		while((type = this.ReadTokenType()) != NbtTokenType.End)
		{
			INbtToken token = this.ReadCurrentToken(type);
			compound.AddChild(token);
		}

		return compound;
	}

	#endregion
}
