namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Internal;

/// <summary>
/// Provides methods for writing a <see cref="NbtCompoundToken"/> to bNBT
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtWriter
{
	/// <summary>
	/// The output stream for write operations.
	/// </summary>
	public Stream DataStream { get; set; }

	/// <summary>
	/// Creates a new <see cref="BinaryNbtWriter"/> instance.
	/// </summary>
	/// <param name="dataStream">Defines the stream the writer will write to.</param>
	public BinaryNbtWriter(Stream dataStream)
	{
		this.DataStream = dataStream;
	}

	/// <summary>
	/// Writes a compound token to the current stream.
	/// </summary>
	/// <param name="root">A <see cref="NbtCompoundToken"/>. Since this is the stream root token, the name may be omitted.</param>
	public void WriteCompound(NbtCompoundToken root) 
	{
		foreach(INbtToken v in root.Children)
		{
			this.WriteToken(v);
		}

		this.WriteByte(0x00);
	}

	/// <summary>
	/// Writes the name of any <see cref="INbtToken"/> to the current stream.
	/// </summary>
	public void WriteName(Byte[] name)
	{
		this.WriteInt16((Int16)name.Length);
		this.DataStream.Write(name);
	}

	/// <summary>
	/// Writes a <see cref="Byte"/> to the current stream.
	/// </summary>
	public void WriteByte(Byte data) => this.DataStream.WriteByte(data);

	/// <summary>
	/// Writes a <see cref="Int16"/> to the current stream.
	/// </summary>
	public void WriteInt16(Int16 data)
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BinaryPrimitives.WriteInt16BigEndian(buffer, data);

		this.DataStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="Int32"/> to the current stream.
	/// </summary>
	public void WriteInt32(Int32 data)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteInt32BigEndian(buffer, data);

		this.DataStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="Int64"/> to the current stream.
	/// </summary>
	public void WriteInt64(Int64 data)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteInt64BigEndian(buffer, data);

		this.DataStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="Single"/> to the current stream.
	/// </summary>
	public void WriteSingle(Single data)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteSingleBigEndian(buffer, data);

		this.DataStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="Double"/> to the current stream.
	/// </summary>
	public void WriteDouble(Double data)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteDoubleBigEndian(buffer, data);

		this.DataStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="String"/> to the current stream.
	/// </summary>
	public void WriteString(String data)
	{
		this.WriteInt16((Int16)data.Length);
		this.DataStream.Write(Encoding.UTF8.GetBytes(data));
	}

	/// <summary>
	/// Writes a <see cref="List{T}"/> of <see cref="Byte"/>s to the current stream.
	/// </summary>
	public void WriteByteArray(List<Byte> data)
	{
		this.WriteInt32(data.Count);
		this.DataStream.Write(data.ToArray());
	}

	/// <summary>
	/// Writes a <see cref="List{T}"/> of <see cref="Int32"/>s to the current stream.
	/// </summary>
	public void WriteInt32Array(List<Int32> data)
	{
		this.WriteInt32(data.Count);

		foreach(Int32 i in data)
		{
			this.WriteInt32(i);
		}
	}

	/// <summary>
	/// Writes a <see cref="List{T}"/> of <see cref="Int32BigEndian"/>s to the current stream.
	/// </summary>
	public void WriteInt32ArrayBigEndian(List<Int32BigEndian> data)
	{
		this.WriteInt32(data.Count);

		this.DataStream.Write(MemoryMarshal.Cast<Int32BigEndian, Byte>(CollectionsMarshal.AsSpan(data)));
	}

	/// <summary>
	/// Writes a <see cref="List{T}"/> of <see cref="Int64"/>s to the current stream.
	/// </summary>
	public void WriteInt64Array(List<Int64> data)
	{
		this.WriteInt32(data.Count);

		foreach(Int64 i in data)
		{
			this.WriteInt64(i);
		}
	}

	/// <summary>
	/// Writes a <see cref="List{T}"/> of <see cref="Int64BigEndian"/>s to the current stream.
	/// </summary>
	public void WriteInt64ArrayBigEndian(List<Int64BigEndian> data)
	{
		this.WriteInt32(data.Count);

		this.DataStream.Write(MemoryMarshal.Cast<Int64BigEndian, Byte>(CollectionsMarshal.AsSpan(data)));
	}

	/// <summary>
	/// Writes a <see cref="NbtListToken"/> to the current stream.
	/// </summary>
	public void WriteList(NbtListToken list)
	{
		this.WriteByte(NbtListToken.Declarator);
		this.WriteByte((Byte)list.ListTokenType);
		this.WriteInt32(list.TargetLength);
		this.WriteName(list.Name);

		foreach(INbtToken v in list.Content)
		{
			this.WriteNamelessToken(v);
		}
	}

	/// <summary>
	/// Writes a <see cref="NbtCompoundToken"/> to the current stream.
	/// </summary>
	public void WriteCompoundToken(NbtCompoundToken compound, Boolean named = true)
	{
		if(named)
		{
			this.WriteByte(NbtCompoundToken.Declarator);
			this.WriteName(compound.Name);
		}
		
		foreach(INbtToken v in compound.Children)
		{
			if(v is not NbtEndToken)
			{
				this.WriteToken(v);
			}
		}

		this.WriteByte(0x00);
	}

	/// <summary>
	/// Writes any <see cref="INbtToken"/> to the current stream.
	/// </summary>
	public void WriteToken(INbtToken token)
	{
		switch(token)
		{
			case NbtByteToken t:
				this.WriteByte(NbtByteToken.Declarator);
				this.WriteName(t.Name);
				this.WriteByte(t.Value);
				break;
			case NbtInt16Token t:
				this.WriteByte(NbtInt16Token.Declarator);
				this.WriteName(t.Name);
				this.WriteInt16(t.Value);
				break;
			case NbtInt32Token t:
				this.WriteByte(NbtInt32Token.Declarator);
				this.WriteName(t.Name);
				this.WriteInt32(t.Value);
				break;
			case NbtInt64Token t:
				this.WriteByte(NbtInt64Token.Declarator);
				this.WriteName(t.Name);
				this.WriteInt64(t.Value);
				break;
			case NbtSingleToken t:
				this.WriteByte(NbtSingleToken.Declarator);
				this.WriteName(t.Name);
				this.WriteSingle(t.Value);
				break;
			case NbtDoubleToken t:
				this.WriteByte(NbtDoubleToken.Declarator);
				this.WriteName(t.Name);
				this.WriteDouble(t.Value);
				break;
			case NbtByteArrayToken t:
				this.WriteByte(NbtByteArrayToken.Declarator);
				this.WriteName(t.Name);
				this.WriteByteArray(t.Elements);
				break;
			case NbtStringToken t:
				this.WriteByte(NbtStringToken.Declarator);
				this.WriteName(t.Name);
				this.WriteString(t.Value);
				break;
			case NbtListToken t:
				this.WriteList(t);
				break;
			case NbtCompoundToken t:
				this.WriteCompoundToken(t);
				break;
			case NbtInt32ArrayToken t:
				this.WriteByte(NbtInt32ArrayToken.Declarator);
				this.WriteName(t.Name);
				this.WriteInt32Array(t.Elements);
				break;
			case NbtBigEndianInt32ArrayToken t:
				this.WriteByte(NbtBigEndianInt32ArrayToken.Declarator);
				this.WriteName(t.Name);
				this.WriteInt32ArrayBigEndian(t.Elements);
				break;
			case NbtInt64ArrayToken t:
				this.WriteByte(NbtInt64ArrayToken.Declarator);
				this.WriteName(t.Name);
				this.WriteInt64Array(t.Elements);
				break;
			case NbtBigEndianInt64ArrayToken t:
				this.WriteByte(NbtBigEndianInt64ArrayToken.Declarator);
				this.WriteName(t.Name);
				this.WriteInt64ArrayBigEndian(t.Elements);
				break;
		}
	}

	/// <summary>
	/// Writes any nameless <see cref="INbtToken"/> to the current stream.
	/// </summary>
	public void WriteNamelessToken(INbtToken token)
	{
		switch(token)
		{
			case NbtByteToken t:
				this.WriteByte(t.Value);
				break;
			case NbtInt16Token t:
				this.WriteInt16(t.Value);
				break;
			case NbtInt32Token t:
				this.WriteInt32(t.Value);
				break;
			case NbtInt64Token t:
				this.WriteInt64(t.Value);
				break;
			case NbtSingleToken t:
				this.WriteSingle(t.Value);
				break;
			case NbtDoubleToken t:
				this.WriteDouble(t.Value);
				break;
			case NbtByteArrayToken t:
				this.WriteByteArray(t.Elements);
				break;
			case NbtStringToken t:
				this.WriteString(t.Value);
				break;
			case NbtCompoundToken t:
				this.WriteCompoundToken(t, false);
				break;
			case NbtInt32ArrayToken t:
				this.WriteInt32Array(t.Elements);
				break;
			case NbtBigEndianInt32ArrayToken t:
				this.WriteInt32ArrayBigEndian(t.Elements);
				break;
			case NbtInt64ArrayToken t:
				this.WriteInt64Array(t.Elements);
				break;
			case NbtBigEndianInt64ArrayToken t:
				this.WriteInt64ArrayBigEndian(t.Elements);
				break;
		}
	}
}
