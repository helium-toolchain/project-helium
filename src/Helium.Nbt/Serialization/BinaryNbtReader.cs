namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;
using Helium.Nbt.Internal;

/// <summary>
/// Allows reading NBT data from a given stream.
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtReader
{
	/// <summary>
	/// Stores the stream to read data from.
	/// </summary>
	public Stream DataStream { get; set; }

	private readonly BinaryReaderEndianness endianness;

	/// <summary>
	/// Creates a new instance of the <see cref="BinaryNbtReader"/> class.
	/// </summary>
	/// <param name="input">The input stream to read data from.</param>
	/// <param name="endianness">Specifies the preferred Endianness of the reader, see <see cref="BinaryReaderEndianness"/>
	/// for more information</param>
	public BinaryNbtReader(Stream input, BinaryReaderEndianness endianness)
	{
		this.DataStream = input;
		this.endianness = endianness;
	}

	/// <summary>
	/// Reads a <see cref="NbtCompoundToken"/> from this stream. Data may follow thereafter.
	/// </summary>
	/// <returns></returns>
	public NbtCompoundToken ReadCompound()
	{
		NbtCompoundToken compound = new();

		NbtTokenType type;

		while((type = this.ReadTokenType()) != NbtTokenType.End)
		{
			compound.AddChild(this.ReadCurrentToken(type));
		}

		return compound;
	}

	/// <summary>
	/// Reads the <see cref="NbtTokenType"/> of the next token.
	/// </summary>
	/// <exception cref="MalformedNbtException"></exception>
	public NbtTokenType ReadTokenType()
	{
		Byte type = (Byte)this.DataStream.ReadByte();

		if(type > (Byte)NbtTokenType.LongArray)
		{
			throw new MalformedNbtException($"Invalid token type found: {type}");
		}

		return (NbtTokenType)type;
	}

	/// <summary>
	/// Reads the next token in the stream.
	/// </summary>
	/// <param name="tokenType">Specifies the <see cref="NbtTokenType"/> to be read.</param>
	/// <param name="readName">Specifies whether a name should be read. Defaults to <see langword="true"/></param>
	/// <exception cref="MalformedNbtException"></exception>
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
			NbtTokenType.Byte => new NbtByteToken(name, (SByte)this.ReadByte()),
			NbtTokenType.Short => new NbtInt16Token(name, this.ReadInt16()),
			NbtTokenType.Int => new NbtInt32Token(name, this.ReadInt32()),
			NbtTokenType.Long => new NbtInt64Token(name, this.ReadInt64()),
			NbtTokenType.Float => new NbtSingleToken(name, this.ReadSingle()),
			NbtTokenType.Double => new NbtDoubleToken(name, this.ReadDouble()),
			NbtTokenType.String => new NbtStringToken(name, this.ReadString()),
			NbtTokenType.Compound => this.ReadCompoundToken(name),
			NbtTokenType.List => this.ReadListToken(name),
			NbtTokenType.ByteArray => this.ReadByteArray(name),
			NbtTokenType.IntArray when endianness == BinaryReaderEndianness.Native => this.ReadInt32Array(name),
			NbtTokenType.IntArray when endianness == BinaryReaderEndianness.BigEndian => this.ReadInt32ArrayBigEndian(name),
			NbtTokenType.LongArray when endianness == BinaryReaderEndianness.Native => this.ReadInt64Array(name),
			NbtTokenType.LongArray when endianness == BinaryReaderEndianness.BigEndian => this.ReadInt64ArrayBigEndian(name),
			_ => throw new MalformedNbtException("Invalid NBT token type.")
		};
	}

	/// <summary>
	/// Reads a <see cref="Byte"/> array from the current stream, indicating the name of the token.
	/// </summary>
	/// <returns></returns>
	public Byte[] ReadName()
	{
		UInt16 length = this.ReadUInt16();
		Span<Byte> buffer = stackalloc Byte[length];

		this.DataStream.Read(buffer);

		return buffer.ToArray();
	}

	public UInt16 ReadUInt16()
	{
		Span<Byte> buffer = stackalloc Byte[2];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadUInt16BigEndian(buffer);
	}

	#region read primitives

	/// <summary>
	/// Reads one <see cref="Byte"/> from the current stream.
	/// </summary>
	public Byte ReadByte() => (Byte)this.DataStream.ReadByte();

	/// <summary>
	/// Reads one <see cref="Int16"/> from the current stream.
	/// </summary>
	public Int16 ReadInt16()
	{
		Span<Byte> buffer = stackalloc Byte[2];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt16BigEndian(buffer);
	}

	/// <summary>
	/// Reads one <see cref="Int32"/> from the current stream.
	/// </summary>
	public Int32 ReadInt32()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt32BigEndian(buffer);
	}

	/// <summary>
	/// Reads one <see cref="Int64"/> from the current stream.
	/// </summary>
	public Int64 ReadInt64()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadInt64BigEndian(buffer);
	}

	/// <summary>
	/// Reads one <see cref="Single"/> from the current stream.
	/// </summary>
	public Single ReadSingle()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadSingleBigEndian(buffer);
	}

	/// <summary>
	/// Reads one <see cref="Double"/> from the current stream.
	/// </summary>
	public Double ReadDouble()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		this.DataStream.Read(buffer);

		return BinaryPrimitives.ReadDoubleBigEndian(buffer);
	}

	/// <summary>
	/// Reads one <see cref="String"/> from the current stream.
	/// </summary>
	public String ReadString()
	{
		UInt16 length = this.ReadUInt16();

		Span<Byte> buffer = new Byte[length];

		this.DataStream.Read(buffer);
		return Encoding.UTF8.GetString(buffer);
	}

	#endregion

	#region read complex types

	/// <summary>
	/// Reads one <see cref="NbtByteArrayToken"/> from the current stream.
	/// </summary>
	public NbtByteArrayToken ReadByteArray(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length];

		this.DataStream.Read(buffer);

		return new NbtByteArrayToken(name, MemoryMarshal.Cast<Byte, SByte>(buffer));
	}

	/// <summary>
	/// Reads one <see cref="NbtInt32ArrayToken"/> from the current stream.
	/// </summary>
	/// <remarks>
	/// This is only called if <see cref="BinaryReaderEndianness.Native"/> was passed to the constructor.
	/// </remarks>
	public NbtInt32ArrayToken ReadInt32Array(Byte[] name)
	{
		Int32 length = this.ReadInt32();
		List<Int32> array = new();

		for(Int32 i = 0; i < length; i++)
		{
			array.Add(this.ReadInt32());
		}

		return new NbtInt32ArrayToken(name, array);
	}

	/// <summary>
	/// Reads one <see cref="NbtBigEndianInt32ArrayToken"/> from the current stream.
	/// </summary>
	/// <remarks>
	/// This is only called if <see cref="BinaryReaderEndianness.BigEndian"/> was passed to the constructor.
	/// </remarks>
	public NbtBigEndianInt32ArrayToken ReadInt32ArrayBigEndian(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length * 4];

		this.DataStream.Read(buffer);

		return new NbtBigEndianInt32ArrayToken(name, MemoryMarshal.Cast<Byte, Int32BigEndian>(buffer));
	}

	/// <summary>
	/// Reads one <see cref="NbtInt64ArrayToken"/> from the current stream.
	/// </summary>
	/// <remarks>
	/// This is only called if <see cref="BinaryReaderEndianness.Native"/> was passed to the constructor.
	/// </remarks>
	public NbtInt64ArrayToken ReadInt64Array(Byte[] name)
	{
		Int32 length = this.ReadInt32();
		List<Int64> array = new();

		for(Int32 i = 0; i < length; i++)
		{
			array.Add(this.ReadInt64());
		}

		return new NbtInt64ArrayToken(name, array);
	}

	/// <summary>
	/// Reads one <see cref="NbtBigEndianInt64ArrayToken"/> from the current stream.
	/// </summary>
	/// <remarks>
	/// This is only called if <see cref="BinaryReaderEndianness.BigEndian"/> was passed to the constructor.
	/// </remarks>
	public NbtBigEndianInt64ArrayToken ReadInt64ArrayBigEndian(Byte[] name)
	{
		Int32 length = this.ReadInt32();

		Span<Byte> buffer = new Byte[length * 8];

		this.DataStream.Read(buffer);

		return new NbtBigEndianInt64ArrayToken(name, MemoryMarshal.Cast<Byte, Int64BigEndian>(buffer));
	}

	/// <summary>
	/// Reads a named <see cref="NbtCompoundToken"/> from the current stream.
	/// </summary>
	public NbtCompoundToken ReadCompoundToken(Byte[] name)
	{
		NbtCompoundToken compound = new(name, new());

		NbtTokenType type;

		while((type = this.ReadTokenType()) != NbtTokenType.End)
		{
			INbtToken token = this.ReadCurrentToken(type);
			compound.AddChild(token);
		}

		return compound;
	}

	/// <summary>
	/// Reads a <see cref="NbtListToken"/> from the current stream.
	/// </summary>
	public NbtListToken ReadListToken(Byte[] name)
	{
		NbtTokenType listType = this.ReadTokenType();
		Int32 length = this.ReadInt32();

		NbtListToken list = new(name, new(), length, listType);

		for(Int32 i = 0; i < length; i++)
		{
			list.Add(this.ReadCurrentToken(listType, false));
		}

		return list;
	}

	#endregion
}
