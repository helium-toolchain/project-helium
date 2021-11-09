namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;
using Helium.Nbt.Internal;

/// <summary>
/// Provides methods for reading a binary NBT stream into a <see cref="NbtCompoundToken"/>
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtReader
{
	private readonly BinaryReaderEndianness endianness;

	public BinaryNbtReader(BinaryReaderEndianness endianness = BinaryReaderEndianness.Native)
	{
		this.endianness = endianness;
	}

	public NbtCompoundToken ReadNbtStream(Stream stream)
	{
		NbtCompoundToken root = new();
		Int16 nameLength = 0, depth = 1;
		IComplexNbtToken? activeToken = root;
		NbtTokenType listTokenType = NbtTokenType.End;
		Int32 listLength = 0;

		do
		{
			if(activeToken is not ITypelessList)
			{
				switch((NbtTokenType)stream.ReadByte())
				{
					case NbtTokenType.End:
						depth -= 1;
						FinalizeToken();
						break;
					case NbtTokenType.Byte:
						AppendToken<NbtByteToken>();
						break;
					case NbtTokenType.Short:
						AppendToken<NbtInt16Token>();
						break;
					case NbtTokenType.Int:
						AppendToken<NbtInt32Token>();
						break;
					case NbtTokenType.Long:
						AppendToken<NbtInt64Token>();
						break;
					case NbtTokenType.Float:
						AppendToken<NbtSingleToken>();
						break;
					case NbtTokenType.Double:
						AppendToken<NbtDoubleToken>();
						break;
					case NbtTokenType.ByteArray:
						AppendToken<NbtByteArrayToken>();
						break;
					case NbtTokenType.IntArray:
						if(this.endianness == BinaryReaderEndianness.Native)
						{
							AppendToken<NbtInt32ArrayToken>();
						}
						else
						{
							AppendToken<NbtBigEndianInt32ArrayToken>();
						}
						break;
					case NbtTokenType.LongArray:
						if(this.endianness == BinaryReaderEndianness.Native)
						{
							AppendToken<NbtInt64ArrayToken>();
						} else
						{
							AppendToken<NbtBigEndianInt64ArrayToken>();
						}
						break;
					case NbtTokenType.List:
						AppendListToken(ReadName());
						break;
					case NbtTokenType.String:
						AppendStringToken(ReadName());
						break;
					case NbtTokenType.Compound:
						AppendCompoundToken(ReadName());
						break;
					default:
						throw new MalformedNbtException("Invalid declared token type");
				}
			} else
			{
				if(activeToken is not ITypelessList list)
				{
					throw new MalformedNbtException("Schrodinger's List token discovered");
				}

				Span<Byte> buffer;
				switch(activeToken)
				{
					case NbtListToken<Byte> b:
						buffer = new Byte[listLength];
						listLength = 0;

						stream.Read(buffer);

						b.Content.AddRange(buffer.ToArray());

						FinalizeListToken();
						continue;

					case NbtListToken<Int16> i16:
						buffer = new Byte[listLength * 2];
						listLength = 0;

						stream.Read(buffer);

						i16.Content.AddRange(MemoryMarshal.Cast<Byte, Int16>(buffer).ToArray());

						FinalizeListToken();
						continue;

					case NbtListToken<Int32> i32:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						i32.Content.AddRange(MemoryMarshal.Cast<Byte, Int32>(buffer).ToArray());

						FinalizeListToken();
						continue;

					case NbtListToken<Int64> i64:
						buffer = new Byte[listLength * 8];
						listLength = 0;

						stream.Read(buffer);

						i64.Content.AddRange(MemoryMarshal.Cast<Byte, Int64>(buffer).ToArray());

						FinalizeListToken();
						continue;

					case NbtListToken<Single> s:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						s.Content.AddRange(MemoryMarshal.Cast<Byte, Single>(buffer).ToArray());

						FinalizeListToken();
						continue;
					case NbtListToken<Double> s:
						buffer = new Byte[listLength * 8];
						listLength = 0;

						stream.Read(buffer);

						s.Content.AddRange(MemoryMarshal.Cast<Byte, Double>(buffer).ToArray());

						FinalizeListToken();
						continue;
				}

				// all primitives are taken care of

				switch(activeToken)
				{
					case NbtListToken<NbtStringToken> s:
						if(s.TargetLength <= s.Count)
						{
							FinalizeListToken();
						}

						AppendStringToken(Encoding.UTF8.GetBytes($"{s.Count}"));
						break;
					case NbtListToken<NbtByteArrayToken> ba:
						if(ba.TargetLength <= ba.Count)
						{
							FinalizeListToken();
						}

						ba.Add(ReadByteArrayToken(Encoding.UTF8.GetBytes($"{ba.Count}")));
						break;
					case NbtListToken<NbtInt32ArrayToken> i32a:
						if(i32a.TargetLength <= i32a.Count)
						{
							FinalizeListToken();
						}

						i32a.Add(ReadInt32ArrayToken(Encoding.UTF8.GetBytes($"{i32a.Count}")));
						break;
					case NbtListToken<NbtBigEndianInt32ArrayToken> bei32a:
						if(bei32a.TargetLength <= bei32a.Count)
						{
							FinalizeListToken();
						}

						bei32a.Add(ReadBigEndianInt32ArrayToken(Encoding.UTF8.GetBytes($"{bei32a.Count}")));
						break;
					case NbtListToken<NbtInt64ArrayToken> i64a:
						if(i64a.TargetLength <= i64a.Count)
						{
							FinalizeListToken();
						}

						i64a.Add(ReadInt64ArrayToken(Encoding.UTF8.GetBytes($"{i64a.Count}")));
						break;
					case NbtListToken<NbtBigEndianInt64ArrayToken> bei64a:
						if(bei64a.TargetLength <= bei64a.Count)
						{
							FinalizeListToken();
						}

						bei64a.Add(ReadBigEndianInt64ArrayToken(Encoding.UTF8.GetBytes($"{bei64a.Count}")));
						break;
					case NbtListToken<ITypelessList> tl:
						if(tl.TargetLength <= tl.Count)
						{
							FinalizeListToken();
						}

						AppendListToken(Encoding.UTF8.GetBytes($"{tl.Count}"));
						break;
					case NbtListToken<NbtCompoundToken> c:
						if(c.TargetLength <= c.Count)
						{
							FinalizeListToken();
						}

						AppendCompoundToken(Encoding.UTF8.GetBytes($"{c.Count}"));
						break;
					default:
						throw new MalformedNbtException("Invalid List token data discovered");
				}

				FinalizeListToken();
			}
		} while(depth > 0);

		return root;

		void FinalizeToken()
		{
			if(activeToken is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("EndToken discovered outside of a CompoundToken");
			}

			token.Children.Add(new NbtEndToken());
			activeToken = token.Parent;
		}

		void FinalizeListToken()
		{
			if(activeToken is not ITypelessList token)
			{
				throw new MalformedNbtException("End of ListToken discovered outside of a ListToken");
			}

			activeToken = activeToken.Parent;
		}

		void AppendToken<T>()
			where T : INbtToken
		{
			if(activeToken is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named Binary Token discovered outside of a CompoundToken");
			}

			switch(default(T))
			{
				case NbtByteToken:
					token.Children.Add(ReadByteToken(stream));
					break;
				case NbtInt16Token:
					token.Children.Add(ReadInt16Token(stream));
					break;
				case NbtInt32Token:
					token.Children.Add(ReadInt32Token(stream));
					break;
				case NbtInt64Token:
					token.Children.Add(ReadInt64Token(stream));
					break;
				case NbtSingleToken:
					token.Children.Add(ReadSingleToken(stream));
					break;
				case NbtDoubleToken:
					token.Children.Add(ReadDoubleToken(stream));
					break;
				case NbtByteArrayToken:
					token.Children.Add(ReadByteArrayToken(ReadName()));
					break;
				case NbtInt32ArrayToken:
					token.Children.Add(ReadInt32ArrayToken(ReadName()));
					break;
				case NbtInt64ArrayToken:
					token.Children.Add(ReadInt64ArrayToken(ReadName()));
					break;
				case NbtBigEndianInt32ArrayToken:
					token.Children.Add(ReadBigEndianInt32ArrayToken(ReadName()));
					break;
				case NbtBigEndianInt64ArrayToken:
					token.Children.Add(ReadBigEndianInt64ArrayToken(ReadName()));
					break;
				default:
					throw new MalformedNbtException("Unknown malformed NBT data.");
			}
		}

		Byte[] ReadName()
		{
			Span<Byte> name = stackalloc Byte[0], buffer = stackalloc Byte[2];

			stream.Read(buffer);
			nameLength = BinaryPrimitives.ReadInt16BigEndian(buffer);

			name = stackalloc Byte[nameLength];

			return name.ToArray();
		}

		NbtByteArrayToken ReadByteArrayToken(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength];
			stream.Read(array);

			return new(name, array, activeToken);
		}

		NbtInt32ArrayToken ReadInt32ArrayToken(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 4];
			stream.Read(array);

			Span<Int32> values = new Int32[arrayLength];

			for(Int32 i = 0, j = 0; i > array.Length; i += 4, j++)
			{
				values[j] = BinaryPrimitives.ReadInt32BigEndian(array.Slice(i, 4));
			}

			return new(name, values, activeToken);
		}

		NbtInt64ArrayToken ReadInt64ArrayToken(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 8];
			stream.Read(array);

			Span<Int64> values = new Int64[arrayLength];

			for(Int32 i = 0, j = 0; i > array.Length; i += 8, j++)
			{
				values[j] = BinaryPrimitives.ReadInt32BigEndian(array.Slice(i, 8));
			}

			return new(name, values, activeToken);
		}

		NbtBigEndianInt32ArrayToken ReadBigEndianInt32ArrayToken(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 4];
			stream.Read(array);

			return new(name, MemoryMarshal.Cast<Byte, Int32BigEndian>(array), activeToken);
		}

		NbtBigEndianInt64ArrayToken ReadBigEndianInt64ArrayToken(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 8];
			stream.Read(array);

			Span<Int64> values = new Int64[arrayLength];

			return new(name, MemoryMarshal.Cast<Byte, Int64BigEndian>(array), activeToken);
		}

		void AppendListToken(Byte[] name)
		{
			Span<Byte> type = stackalloc Byte[1], length = stackalloc Byte[4];

			stream.Read(type);
			stream.Read(length);

			listTokenType = (NbtTokenType)type[0];
			listLength = BinaryPrimitives.ReadInt32BigEndian(length);

			IComplexNbtToken listToken;

			switch(listTokenType)
			{
				case NbtTokenType.End:
					if(listLength > 0)
					{
						throw new MalformedNbtException("List of End tags discovered");
					}
					listToken = new NbtListToken<NbtEndToken>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Byte:
					listToken = new NbtListToken<Byte>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Short:
					listToken = new NbtListToken<Int16>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Int:
					listToken = new NbtListToken<Int32>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Long:
					listToken = new NbtListToken<Int64>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Float:
					listToken = new NbtListToken<Single>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Double:
					listToken = new NbtListToken<Double>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.ByteArray:
					listToken = new NbtListToken<NbtByteArrayToken>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.String:
					listToken = new NbtListToken<NbtStringToken>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.List:
					listToken = new NbtListToken<ITypelessList>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.Compound:
					listToken = new NbtListToken<NbtCompoundToken>(name, new(), activeToken, listLength);
					break;
				case NbtTokenType.IntArray:
					if(this.endianness == BinaryReaderEndianness.Native)
					{
						listToken = new NbtListToken<NbtInt32ArrayToken>(name, new(), activeToken, listLength);
					} 
					else
					{
						listToken = new NbtListToken<NbtBigEndianInt32ArrayToken>(name, new(), activeToken, listLength);
					}
					break;
				case NbtTokenType.LongArray:
					if(this.endianness == BinaryReaderEndianness.Native)
					{
						listToken = new NbtListToken<NbtInt64ArrayToken>(name, new(), activeToken, listLength);
					} 
					else
					{
						listToken = new NbtListToken<NbtBigEndianInt64ArrayToken>(name, new(), activeToken, listLength);
					}
					break;
				default:
					throw new MalformedNbtException("Invalid token type declaring a List token");
			}

			if(activeToken is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named list token discovered outside of a CompoundToken");
			}

			token.Children.Add(listToken);

			activeToken = listToken;
			depth++;
		}

		void AppendStringToken(Byte[] name)
		{
			if(activeToken is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named string token discovered outside of a CompoundToken");
			}

			Span<Byte> buffer = stackalloc Byte[2], array;

			stream.Read(buffer);

			nameLength = BinaryPrimitives.ReadInt16BigEndian(buffer); // we reuse nameLength for the string length
			array = new Byte[nameLength];

			stream.Read(array);

			token.Children.Add(new NbtStringToken(name, array, activeToken));
		}

		void AppendCompoundToken(Byte[] name)
		{
			NbtCompoundToken compound = new(name, new(), activeToken);
			compound.Parent = activeToken;

			if(activeToken is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named compound token discovered outside of a CompoundToken");
			}

			token.Children.Add(compound);

			activeToken = compound;

			depth++;
		}
	}

	#region Reading primitives

	/// <summary>
	/// Reads one <see cref="NbtByteToken"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtByteToken ReadByteToken(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[1];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new()
		{
			Name = name.ToArray(),
			Value = payload[0]
		};
	}

	/// <summary>
	/// Reads one <see cref="NbtInt16Token"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtInt16Token ReadInt16Token(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[2];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new() 
		{ 
			Name = name.ToArray(),
			Value = BinaryPrimitives.ReadInt16BigEndian(payload) 
		};
	}

	/// <summary>
	/// Reads one <see cref="NbtInt32Token"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtInt32Token ReadInt32Token(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[4];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new()
		{
			Name = name.ToArray(),
			Value = BinaryPrimitives.ReadInt32BigEndian(payload)
		};
	}

	/// <summary>
	/// Reads one <see cref="NbtInt64Token"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtInt64Token ReadInt64Token(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[8];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new()
		{
			Name = name.ToArray(),
			Value = BinaryPrimitives.ReadInt64BigEndian(payload)
		};
	}

	/// <summary>
	/// Reads one <see cref="NbtSingleToken"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtSingleToken ReadSingleToken(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[4];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new()
		{
			Name = name.ToArray(),
			Value = BinaryPrimitives.ReadSingleBigEndian(payload)
		};
	}

	/// <summary>
	/// Reads one <see cref="NbtDoubleToken"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtDoubleToken ReadDoubleToken(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[8];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new()
		{
			Name = name.ToArray(),
			Value = BinaryPrimitives.ReadDoubleBigEndian(payload)
		};
	}

	#endregion
}
