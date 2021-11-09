namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for reading a binary NBT stream into a <see cref="NbtCompoundToken"/>
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtReader
{
	public NbtCompoundToken ReadNbtStream(Stream stream)
	{
		NbtCompoundToken root = new();
		Int16 nameLength = 0, depth = 1;
		IComplexNbtToken? activeTag = root;
		NbtTokenType listTagType = NbtTokenType.End;
		Int32 listLength = 0;

		do
		{
			if(activeTag is not ITypelessList)
			{
				switch((NbtTokenType)stream.ReadByte())
				{
					case NbtTokenType.End:
						depth -= 1;
						FinalizeTag();
						break;
					case NbtTokenType.Byte:
						AppendTag<NbtByteToken>();
						break;
					case NbtTokenType.Short:
						AppendTag<NbtInt16Token>();
						break;
					case NbtTokenType.Int:
						AppendTag<NbtInt32Token>();
						break;
					case NbtTokenType.Long:
						AppendTag<NbtInt64Token>();
						break;
					case NbtTokenType.Float:
						AppendTag<NbtSingleToken>();
						break;
					case NbtTokenType.Double:
						AppendTag<NbtDoubleToken>();
						break;
					case NbtTokenType.ByteArray:
						AppendTag<NbtByteArrayToken>();
						break;
					case NbtTokenType.IntArray:
						AppendTag<NbtInt32ArrayToken>();
						break;
					case NbtTokenType.LongArray:
						AppendTag<NbtInt64ArrayToken>();
						break;
					case NbtTokenType.List:
						AppendListTag(ReadName());
						break;
					case NbtTokenType.String:
						AppendStringTag(ReadName());
						break;
					case NbtTokenType.Compound:
						AppendCompoundTag(ReadName());
						break;
					default:
						throw new MalformedNbtException("Invalid declared token type");
				}
			} else
			{
				if(activeTag is not ITypelessList list)
				{
					throw new MalformedNbtException("Schrodinger's List token discovered");
				}

				Span<Byte> buffer;
				switch(activeTag)
				{
					case NbtListToken<Byte> b:
						buffer = new Byte[listLength];
						listLength = 0;

						stream.Read(buffer);

						b.Content.AddRange(buffer.ToArray());

						FinalizeListTag();
						continue;

					case NbtListToken<Int16> i16:
						buffer = new Byte[listLength * 2];
						listLength = 0;

						stream.Read(buffer);

						i16.Content.AddRange(MemoryMarshal.Cast<Byte, Int16>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case NbtListToken<Int32> i32:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						i32.Content.AddRange(MemoryMarshal.Cast<Byte, Int32>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case NbtListToken<Int64> i64:
						buffer = new Byte[listLength * 8];
						listLength = 0;

						stream.Read(buffer);

						i64.Content.AddRange(MemoryMarshal.Cast<Byte, Int64>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case NbtListToken<Single> s:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						s.Content.AddRange(MemoryMarshal.Cast<Byte, Single>(buffer).ToArray());

						FinalizeListTag();
						continue;
					case NbtListToken<Double> s:
						buffer = new Byte[listLength * 8];
						listLength = 0;

						stream.Read(buffer);

						s.Content.AddRange(MemoryMarshal.Cast<Byte, Double>(buffer).ToArray());

						FinalizeListTag();
						continue;
				}

				// all primitives are taken care of

				switch(activeTag)
				{
					case NbtListToken<NbtStringToken> s:
						if(s.TargetLength <= s.Count)
						{
							FinalizeListTag();
						}

						AppendStringTag(Encoding.UTF8.GetBytes($"{s.Count}"));
						break;
					case NbtListToken<NbtByteArrayToken> ba:
						if(ba.TargetLength <= ba.Count)
						{
							FinalizeListTag();
						}

						ba.Add(ReadByteArrayTag(Encoding.UTF8.GetBytes($"{ba.Count}")));
						break;
					case NbtListToken<NbtInt32ArrayToken> i32a:
						if(i32a.TargetLength <= i32a.Count)
						{
							FinalizeListTag();
						}

						i32a.Add(ReadInt32ArrayTag(Encoding.UTF8.GetBytes($"{i32a.Count}")));
						break;
					case NbtListToken<NbtInt64ArrayToken> i64a:
						if(i64a.TargetLength <= i64a.Count)
						{
							FinalizeListTag();
						}

						i64a.Add(ReadInt64ArrayTag(Encoding.UTF8.GetBytes($"{i64a.Count}")));
						break;
					case NbtListToken<ITypelessList> tl:
						if(tl.TargetLength <= tl.Count)
						{
							FinalizeListTag();
						}

						AppendListTag(Encoding.UTF8.GetBytes($"{tl.Count}"));
						break;
					case NbtListToken<NbtCompoundToken> c:
						if(c.TargetLength <= c.Count)
						{
							FinalizeListTag();
						}

						AppendCompoundTag(Encoding.UTF8.GetBytes($"{c.Count}"));
						break;
					default:
						throw new MalformedNbtException("Invalid List token data discovered");
				}

				FinalizeListTag();
			}
		} while(depth > 0);

		return root;

		void FinalizeTag()
		{
			if(activeTag is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("EndTag discovered outside of a CompoundTag");
			}

			token.Children.Add(new NbtEndToken());
			activeTag = token.Parent;
		}

		void FinalizeListTag()
		{
			if(activeTag is not ITypelessList token)
			{
				throw new MalformedNbtException("End of ListTag discovered outside of a ListTag");
			}

			activeTag = activeTag.Parent;
		}

		void AppendTag<T>()
			where T : INbtToken
		{
			if(activeTag is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named Binary Token discovered outside of a CompoundTag");
			}

			switch(default(T))
			{
				case NbtByteToken:
					token.Children.Add(ReadByteTag(stream));
					break;
				case NbtInt16Token:
					token.Children.Add(ReadInt16Tag(stream));
					break;
				case NbtInt32Token:
					token.Children.Add(ReadInt32Tag(stream));
					break;
				case NbtInt64Token:
					token.Children.Add(ReadInt64Tag(stream));
					break;
				case NbtSingleToken:
					token.Children.Add(ReadSingleTag(stream));
					break;
				case NbtDoubleToken:
					token.Children.Add(ReadDoubleTag(stream));
					break;
				case NbtByteArrayToken:
					token.Children.Add(ReadByteArrayTag(ReadName()));
					break;
				case NbtInt32ArrayToken:
					token.Children.Add(ReadInt32ArrayTag(ReadName()));
					break;
				case NbtInt64ArrayToken:
					token.Children.Add(ReadInt64ArrayTag(ReadName()));
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

		NbtByteArrayToken ReadByteArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength];
			stream.Read(array);

			return new(name, array, activeTag);
		}

		NbtInt32ArrayToken ReadInt32ArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 4];
			stream.Read(array);

			return new(name, MemoryMarshal.Cast<Byte, Int32>(array), activeTag);
		}

		NbtInt64ArrayToken ReadInt64ArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 8];
			stream.Read(array);

			return new(name, MemoryMarshal.Cast<Byte, Int64>(array), activeTag);
		}

		void AppendListTag(Byte[] name)
		{
			Span<Byte> type = stackalloc Byte[1], length = stackalloc Byte[4];

			stream.Read(type);
			stream.Read(length);

			listTagType = (NbtTokenType)type[0];
			listLength = BinaryPrimitives.ReadInt32BigEndian(length);

			IComplexNbtToken listTag;

			switch(listTagType)
			{
				case NbtTokenType.End:
					if(listLength > 0)
					{
						throw new MalformedNbtException("List of End tags discovered");
					}
					listTag = new NbtListToken<NbtEndToken>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Byte:
					listTag = new NbtListToken<Byte>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Short:
					listTag = new NbtListToken<Int16>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Int:
					listTag = new NbtListToken<Int32>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Long:
					listTag = new NbtListToken<Int64>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Float:
					listTag = new NbtListToken<Single>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Double:
					listTag = new NbtListToken<Double>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.ByteArray:
					listTag = new NbtListToken<NbtByteArrayToken>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.String:
					listTag = new NbtListToken<NbtStringToken>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.List:
					listTag = new NbtListToken<ITypelessList>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.Compound:
					listTag = new NbtListToken<NbtCompoundToken>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.IntArray:
					listTag = new NbtListToken<NbtInt32ArrayToken>(name, new(), activeTag, listLength);
					break;
				case NbtTokenType.LongArray:
					listTag = new NbtListToken<NbtInt64ArrayToken>(name, new(), activeTag, listLength);
					break;
				default:
					throw new MalformedNbtException("Invalid token type declaring a List token");
			}

			if(activeTag is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named list token discovered outside of a CompoundTag");
			}

			token.Children.Add(listTag);

			activeTag = listTag;
			depth++;
		}

		void AppendStringTag(Byte[] name)
		{
			if(activeTag is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named string token discovered outside of a CompoundTag");
			}

			Span<Byte> buffer = stackalloc Byte[2], array;

			stream.Read(buffer);

			nameLength = BinaryPrimitives.ReadInt16BigEndian(buffer); // we reuse nameLength for the string length
			array = new Byte[nameLength];

			stream.Read(array);

			token.Children.Add(new NbtStringToken(name, array, activeTag));
		}

		void AppendCompoundTag(Byte[] name)
		{
			NbtCompoundToken compound = new(name, new(), activeTag);
			compound.Parent = activeTag;

			if(activeTag is not NbtCompoundToken token)
			{
				throw new MalformedNbtException("Named compound token discovered outside of a CompoundTag");
			}

			token.Children.Add(compound);

			activeTag = compound;

			depth++;
		}
	}

	#region Reading primitives

	/// <summary>
	/// Reads one <see cref="NbtByteToken"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public NbtByteToken ReadByteTag(Stream stream)
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
	public NbtInt16Token ReadInt16Tag(Stream stream)
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
	public NbtInt32Token ReadInt32Tag(Stream stream)
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
	public NbtInt64Token ReadInt64Tag(Stream stream)
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
	public NbtSingleToken ReadSingleTag(Stream stream)
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
	public NbtDoubleToken ReadDoubleTag(Stream stream)
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
