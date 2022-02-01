namespace Helium.Data.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;

/// <summary>
/// Provides a mechanism for writing NBT data to binary data.
/// </summary>
[RequiresPreviewFeatures]
public sealed class BinaryNbtWriter
{
	public Byte[] Serialize(NbtRootToken root)
	{
		MemoryStream stream = new();

		foreach(IDataToken token in root.Children)
		{
			this.writeToken(token, stream);
		}

		stream.WriteByte(0x00);
		return stream.ToArray();
	}

	private void writeToken(IDataToken token, MemoryStream stream)
	{
		switch((NbtTokenType)token.RefDeclarator)
		{
			case NbtTokenType.SByte:
				stream.WriteByte(NbtSByteToken.Declarator);
				this.writeString(token.Name, stream);
				stream.WriteByte((Byte)((NbtSByteToken)token).Value);
				break;

			case NbtTokenType.Int16:
				stream.WriteByte(NbtInt16Token.Declarator);
				this.writeString(token.Name, stream);
				this.writeInt16(((NbtInt16Token)token).Value, stream);
				break;

			case NbtTokenType.Int32:
				stream.WriteByte(NbtInt32Token.Declarator);
				this.writeString(token.Name, stream);
				this.writeInt32(((NbtInt32Token)token).Value, stream);
				break;

			case NbtTokenType.Int64:
				stream.WriteByte(NbtInt64Token.Declarator);
				this.writeString(token.Name, stream);
				this.writeInt64(((NbtInt64Token)token).Value, stream);
				break;

			case NbtTokenType.Single:
				stream.WriteByte(NbtSingleToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeSingle(((NbtSingleToken)token).Value, stream);
				break;

			case NbtTokenType.Double:
				stream.WriteByte(NbtDoubleToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeDouble(((NbtDoubleToken)token).Value, stream);
				break;

			case NbtTokenType.String:
				stream.WriteByte(NbtStringToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeString(((NbtStringToken)token).Value, stream);
				break;

			case NbtTokenType.SByteArray:
				stream.WriteByte(NbtSByteArrayToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeSByteArray((NbtSByteArrayToken)token, stream);
				break;

			case NbtTokenType.Int32Array:
				stream.WriteByte(NbtInt32ArrayToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeInt32Array((NbtInt32ArrayToken)token, stream);
				break;

			case NbtTokenType.Int64Array:
				stream.WriteByte(NbtInt64ArrayToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeInt64Array((NbtInt64ArrayToken)token, stream);
				break;

			case NbtTokenType.Compound:
				stream.WriteByte(NbtCompoundToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeCompound((NbtCompoundToken)token, stream);
				break;

			case NbtTokenType.List:
				stream.WriteByte(NbtListToken.Declarator);
				this.writeString(token.Name, stream);
				this.writeList((NbtListToken)token, stream);
				break;
		}
	}

	private void writeNamelessToken(IDataToken token, MemoryStream stream)
	{
		switch((NbtTokenType)token.RefDeclarator)
		{
			case NbtTokenType.SByte:
				stream.WriteByte((Byte)((NbtSByteToken)token).Value);
				break;

			case NbtTokenType.Int16:
				this.writeInt16(((NbtInt16Token)token).Value, stream);
				break;

			case NbtTokenType.Int32:
				this.writeInt32(((NbtInt32Token)token).Value, stream);
				break;

			case NbtTokenType.Int64:
				this.writeInt64(((NbtInt64Token)token).Value, stream);
				break;

			case NbtTokenType.Single:
				this.writeSingle(((NbtSingleToken)token).Value, stream);
				break;

			case NbtTokenType.Double:
				this.writeDouble(((NbtDoubleToken)token).Value, stream);
				break;

			case NbtTokenType.String:
				this.writeString(((NbtStringToken)token).Value, stream);
				break;

			case NbtTokenType.SByteArray:
				this.writeSByteArray((NbtSByteArrayToken)token, stream);
				break;

			case NbtTokenType.Int32Array:
				this.writeInt32Array((NbtInt32ArrayToken)token, stream);
				break;

			case NbtTokenType.Int64Array:
				this.writeInt64Array((NbtInt64ArrayToken)token, stream);
				break;

			case NbtTokenType.Compound:
				this.writeCompound((NbtCompoundToken)token, stream);
				break;

			case NbtTokenType.List:
				this.writeList((NbtListToken)token, stream);
				break;
		}
	}

	private void writeInt16(Int16 value, MemoryStream stream)
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BinaryPrimitives.WriteInt16BigEndian(buffer, value);

		stream.Write(buffer);
	}

	private void writeInt32(Int32 value, MemoryStream stream)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteInt32BigEndian(buffer, value);

		stream.Write(buffer);
	}

	private void writeInt64(Int64 value, MemoryStream stream)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteInt64BigEndian(buffer, value);

		stream.Write(buffer);
	}

	private void writeSingle(Single value, MemoryStream stream)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteSingleBigEndian(buffer, value);

		stream.Write(buffer);
	}

	private void writeDouble(Double value, MemoryStream stream)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteDoubleBigEndian(buffer, value);

		stream.Write(buffer);
	}

	private void writeString(String value, MemoryStream stream)
	{
		this.writeInt16((Int16)value.Length, stream);
		stream.Write(Encoding.UTF8.GetBytes(value));
	}

	private void writeCompound(NbtCompoundToken compound, MemoryStream stream)
	{
		foreach(IDataToken token in compound.Children)
		{
			this.writeToken(token, stream);
		}

		stream.WriteByte(0x00);
	}

	private void writeSByteArray(NbtSByteArrayToken token, MemoryStream stream)
	{
		this.writeInt32(token.children.Count, stream);
		stream.Write(MemoryMarshal.Cast<SByte, Byte>(CollectionsMarshal.AsSpan(token.children)));
	}

	private void writeInt32Array(NbtInt32ArrayToken token, MemoryStream stream)
	{
		this.writeInt32(token.children.Count, stream);
		foreach(Int32 i in token.children)
		{
			this.writeInt32(i, stream);
		}
	}

	private void writeInt64Array(NbtInt64ArrayToken token, MemoryStream stream)
	{
		this.writeInt32(token.children.Count, stream);
		foreach(Int64 i in token.children)
		{
			this.writeInt64(i, stream);
		}
	}

	private void writeList(NbtListToken token, MemoryStream stream)
	{
		stream.WriteByte(NbtListToken.Declarator);
		stream.WriteByte(token.ListTypeDeclarator);
		this.writeInt32(token.Count, stream);
		this.writeString(token.Name, stream);

		foreach(IDataToken v in token.children)
		{
			this.writeNamelessToken(v, stream);
		}
	}
}
