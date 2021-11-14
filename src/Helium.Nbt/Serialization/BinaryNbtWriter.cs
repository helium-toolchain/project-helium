namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.Versioning;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for writing a <see cref="NbtCompoundToken"/> to NBT
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtWriter
{
	public void WriteNbtStream(Stream stream, NbtCompoundToken root) 
	{
		IComplexNbtToken currentToken = root;
		Boolean writeStart = true;
		Span<Byte> listLength = stackalloc Byte[4];

		do
		{
			if(writeStart)
			{
				switch(currentToken)
				{
					case NbtCompoundToken:
						stream.WriteByte(NbtCompoundToken.Declarator);
						break;
					case ITypelessList l:
						stream.WriteByte(NbtListToken.Declarator);
						stream.WriteByte((Byte)l.ListTokenType);

						BinaryPrimitives.WriteInt32BigEndian(listLength, l.TargetLength);

						stream.Write(listLength);
						break;
					default:
						throw new MalformedDataException("Tried to write the start of a non-complex tag");
				}
			}
		} while(true);
	}

	/// <summary>
	/// Writes a <see cref="NbtByteToken"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtByteToken"/> to write. Must declare a valid <see cref="NbtByteToken.Name"/>
	/// and <see cref="NbtByteToken.Value"/>.</param>
	public void WriteByteToken(Stream stream, NbtByteToken token)
	{
		Span<Byte> length = stackalloc Byte[2];

		stream.WriteByte(NbtByteToken.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(length, (Int16)token.Name.Length);

		stream.Write(length);
		stream.Write(token.Name);
		stream.WriteByte(token.Value);
	}

	/// <summary>
	/// Writes a <see cref="NbtInt16Token"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtInt16Token"/> to write. Must declare a valid <see cref="NbtInt16Token.Name"/>
	/// and <see cref="NbtInt16Token.Value"/>.</param>
	public void WriteInt16Token(Stream stream, NbtInt16Token token)
	{
		Span<Byte> value = stackalloc Byte[2];

		stream.WriteByte(NbtInt16Token.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(value, (Int16)token.Name.Length);

		stream.Write(value);
		stream.Write(token.Name);

		BinaryPrimitives.WriteInt16BigEndian(value, token.Value);

		stream.Write(value);
	}

	/// <summary>
	/// Writes a <see cref="NbtInt32Token"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtInt32Token"/> to write. Must declare a valid <see cref="NbtInt32Token.Name"/>
	/// and <see cref="NbtInt32Token.Value"/>.</param>
	public void WriteInt32Token(Stream stream, NbtInt32Token token)
	{
		Span<Byte> length = stackalloc Byte[2], value = stackalloc Byte[4];

		stream.WriteByte(NbtInt32Token.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(length, (Int16)token.Name.Length);
		BinaryPrimitives.WriteInt32BigEndian(value, token.Value);

		stream.Write(length);
		stream.Write(token.Name);
		stream.Write(value);
	}

	/// <summary>
	/// Writes a <see cref="NbtInt64Token"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtInt64Token"/> to write. Must declare a valid <see cref="NbtInt64Token.Name"/>
	/// and <see cref="NbtInt64Token.Value"/>.</param>
	public void WriteInt64Token(Stream stream, NbtInt64Token token)
	{
		Span<Byte> length = stackalloc Byte[2], value = stackalloc Byte[8];

		stream.WriteByte(NbtInt32Token.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(length, (Int16)token.Name.Length);
		BinaryPrimitives.WriteInt64BigEndian(value, token.Value);

		stream.Write(length);
		stream.Write(token.Name);
		stream.Write(value);
	}

	/// <summary>
	/// Writes a <see cref="NbtSingleToken"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtSingleToken"/> to write. Must declare a valid <see cref="NbtSingleToken.Name"/>
	/// and <see cref="NbtSingleToken.Value"/>.</param>
	public void WriteSingleToken(Stream stream, NbtSingleToken token)
	{
		Span<Byte> length = stackalloc Byte[2], value = stackalloc Byte[4];

		stream.WriteByte(NbtInt32Token.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(length, (Int16)token.Name.Length);
		BinaryPrimitives.WriteSingleBigEndian(value, token.Value);

		stream.Write(length);
		stream.Write(token.Name);
		stream.Write(value);
	}

	/// <summary>
	/// Writes a <see cref="NbtDoubleToken"/> to the current stream.
	/// </summary>
	/// <param name="stream">The data stream to be written to. Advances by the number of bytes written in total.</param>
	/// <param name="token">The <see cref="NbtDoubleToken"/> to write. Must declare a valid <see cref="NbtDoubleToken.Name"/>
	/// and <see cref="NbtDoubleToken.Value"/>.</param>
	public void WriteDoubleToken(Stream stream, NbtDoubleToken token)
	{
		Span<Byte> length = stackalloc Byte[2], value = stackalloc Byte[8];

		stream.WriteByte(NbtInt32Token.Declarator);

		BinaryPrimitives.WriteInt16BigEndian(length, (Int16)token.Name.Length);
		BinaryPrimitives.WriteDoubleBigEndian(value, token.Value);

		stream.Write(length);
		stream.Write(token.Name);
		stream.Write(value);
	}
}
