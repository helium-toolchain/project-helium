namespace Helium.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for reading a binary NBT stream into a <see cref="CompoundTag"/>
/// </summary>
[RequiresPreviewFeatures]
public class BinaryNbtReader
{
	public CompoundTag ReadNbtStream(Stream stream)
	{
		CompoundTag root = new();
		Int16 nameLength = 0, depth = 1;
		NbtTag? activeTag = root;
		NbtTagType listTagType = NbtTagType.End;
		Int32 listLength = 0;

		do
		{
			if(activeTag is not ITypelessList)
			{
				switch((NbtTagType)stream.ReadByte())
				{
					case NbtTagType.End:
						depth -= 1;
						FinalizeTag();
						break;
					case NbtTagType.Byte:
						AppendTag<ByteTag>();
						break;
					case NbtTagType.Short:
						AppendTag<Int16Tag>();
						break;
					case NbtTagType.Int:
						AppendTag<Int32Tag>();
						break;
					case NbtTagType.Long:
						AppendTag<Int64Tag>();
						break;
					case NbtTagType.Float:
						AppendTag<FloatTag>();
						break;
					case NbtTagType.Double:
						AppendTag<DoubleTag>();
						break;
					case NbtTagType.ByteArray:
						AppendTag<ByteArrayTag>();
						break;
					case NbtTagType.IntArray:
						AppendTag<Int32ArrayTag>();
						break;
					case NbtTagType.LongArray:
						AppendTag<Int64ArrayTag>();
						break;
					case NbtTagType.List:
						AppendListTag(ReadName());
						break;
					case NbtTagType.String:
						AppendStringTag(ReadName());
						break;
					case NbtTagType.Compound:
						AppendCompoundTag(ReadName());
						break;
					default:
						throw new MalformedNbtException("Invalid declared tag type");
				}
			} 
			else
			{
				if(activeTag is not ITypelessList list)
				{
					throw new MalformedNbtException("Schrodinger's List tag discovered");
				}

				Span<Byte> buffer;
				switch(activeTag)
				{
					case ListTag<Byte> b:
						buffer = new Byte[listLength];
						listLength = 0;

						stream.Read(buffer);

						b.Content.AddRange(buffer.ToArray());

						FinalizeListTag();
						continue;

					case ListTag<Int16> i16:
						buffer = new Byte[listLength * 2];
						listLength = 0;

						stream.Read(buffer);

						i16.Content.AddRange(MemoryMarshal.Cast<Byte, Int16>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case ListTag<Int32> i32:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						i32.Content.AddRange(MemoryMarshal.Cast<Byte, Int32>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case ListTag<Int64> i64:
						buffer = new Byte[listLength * 8];
						listLength = 0;

						stream.Read(buffer);

						i64.Content.AddRange(MemoryMarshal.Cast<Byte, Int64>(buffer).ToArray());

						FinalizeListTag();
						continue;

					case ListTag<Single> s:
						buffer = new Byte[listLength * 4];
						listLength = 0;

						stream.Read(buffer);

						s.Content.AddRange(MemoryMarshal.Cast<Byte, Single>(buffer).ToArray());

						FinalizeListTag();
						continue;
					case ListTag<Double> s:
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
					case ListTag<StringTag> s:
						if(s.Length <= s.Count)
						{
							FinalizeListTag();
						}

						AppendStringTag(Encoding.UTF8.GetBytes($"{s.Count}"));
						break;
					case ListTag<ByteArrayTag> ba:
						if(ba.Length <= ba.Count)
						{
							FinalizeListTag();
						}

						ba.Add(ReadByteArrayTag(Encoding.UTF8.GetBytes($"{ba.Count}")));
						break;
					case ListTag<Int32ArrayTag> i32a:
						if(i32a.Length <= i32a.Count)
						{
							FinalizeListTag();
						}

						i32a.Add(ReadInt32ArrayTag(Encoding.UTF8.GetBytes($"{i32a.Count}")));
						break;
					case ListTag<Int64ArrayTag> i64a:
						if(i64a.Length <= i64a.Count)
						{
							FinalizeListTag();
						}

						i64a.Add(ReadInt64ArrayTag(Encoding.UTF8.GetBytes($"{i64a.Count}")));
						break;
					case ListTag<ITypelessList> tl:
						if(tl.Length <= tl.Count)
						{
							FinalizeListTag();
						}

						AppendListTag(Encoding.UTF8.GetBytes($"{tl.Count}"));
						break;
					case ListTag<CompoundTag> c:
						if(c.Length <= c.Count)
						{
							FinalizeListTag();
						}

						AppendCompoundTag(Encoding.UTF8.GetBytes($"{c.Count}"));
						break;
					default:
						throw new MalformedNbtException("Invalid List tag data discovered");
				}

				FinalizeListTag();
			}
		} while(depth > 0);

		return root;

		void FinalizeTag()
		{
			if(activeTag is not CompoundTag tag)
			{
				throw new MalformedNbtException("EndTag discovered outside of a CompoundTag");
			}

			tag.Children.Add(new EndTag());
			activeTag = tag.Parent;
		}

		void FinalizeListTag()
		{
			if(activeTag is not ITypelessList tag)
			{
				throw new MalformedNbtException("End of ListTag discovered outside of a ListTag");
			}

			activeTag = activeTag.Parent;
		}

		void AppendTag<T>()
			where T : INbtToken
		{
			if(activeTag is not CompoundTag tag)
			{
				throw new MalformedNbtException("Named Binary Tag discovered outside of a CompoundTag");
			}

			switch(default(T))
			{
				case ByteTag:
					tag.Children.Add(ReadByteTag(stream));
					break;
				case Int16Tag:
					tag.Children.Add(ReadInt16Tag(stream));
					break;
				case Int32Tag:
					tag.Children.Add(ReadInt32Tag(stream));
					break;
				case Int64Tag:
					tag.Children.Add(ReadInt64Tag(stream));
					break;
				case FloatTag:
					tag.Children.Add(ReadFloatTag(stream));
					break;
				case DoubleTag:
					tag.Children.Add(ReadDoubleTag(stream));
					break;
				case ByteArrayTag:
					tag.Children.Add(ReadByteArrayTag(ReadName()));
					break;
				case Int32ArrayTag:
					tag.Children.Add(ReadInt32ArrayTag(ReadName()));
					break;
				case Int64ArrayTag:
					tag.Children.Add(ReadInt64ArrayTag(ReadName()));
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

		ByteArrayTag ReadByteArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength];
			stream.Read(array);

			return new(name, array);
		}

		Int32ArrayTag ReadInt32ArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 4];
			stream.Read(array);

			return new(name, MemoryMarshal.Cast<Byte, Int32>(array));
		}

		Int64ArrayTag ReadInt64ArrayTag(Byte[] name)
		{
			Span<Byte> buffer = stackalloc Byte[4], array;
			Int32 arrayLength;

			stream.Read(buffer);
			arrayLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

			array = new Byte[arrayLength * 8];
			stream.Read(array);

			return new(name, MemoryMarshal.Cast<Byte, Int64>(array));
		}

		void AppendListTag(Byte[] name)
		{
			Span<Byte> type = stackalloc Byte[1], length = stackalloc Byte[4];

			stream.Read(type);
			stream.Read(length);

			listTagType = (NbtTagType)type[0];
			listLength = BinaryPrimitives.ReadInt32BigEndian(length);

			NbtTag listTag;

			switch(listTagType)
			{
				case NbtTagType.End:
					if(listLength > 0)
					{
						throw new MalformedNbtException("List of End tags discovered");
					}
					listTag = new ListTag<EndTag>(name, new(), listLength);
					break;
				case NbtTagType.Byte:
					listTag = new ListTag<Byte>(name, new(), listLength);
					break;
				case NbtTagType.Short:
					listTag = new ListTag<Int16>(name, new(), listLength);
					break;
				case NbtTagType.Int:
					listTag = new ListTag<Int32>(name, new(), listLength);
					break;
				case NbtTagType.Long:
					listTag = new ListTag<Int64>(name, new(), listLength);
					break;
				case NbtTagType.Float:
					listTag = new ListTag<Single>(name, new(), listLength);
					break;
				case NbtTagType.Double:
					listTag = new ListTag<Double>(name, new(), listLength);
					break;
				case NbtTagType.ByteArray:
					listTag = new ListTag<ByteArrayTag>(name, new(), listLength);
					break;
				case NbtTagType.String:
					listTag = new ListTag<StringTag>(name, new(), listLength);
					break;
				case NbtTagType.List:
					listTag = new ListTag<ITypelessList>(name, new(), listLength);
					break;
				case NbtTagType.Compound:
					listTag = new ListTag<CompoundTag>(name, new(), listLength);
					break;
				case NbtTagType.IntArray:
					listTag = new ListTag<Int32ArrayTag>(name, new(), listLength);
					break;
				case NbtTagType.LongArray:
					listTag = new ListTag<Int64ArrayTag>(name, new(), listLength);
					break;
				default:
					throw new MalformedNbtException("Invalid tag type declaring a List tag");
			}

			if(activeTag is not CompoundTag tag)
			{
				throw new MalformedNbtException("Named list tag discovered outside of a CompoundTag");
			}

			tag.Children.Add(listTag);

			listTag.Parent = activeTag;
			activeTag = listTag;
			depth++;
		}

		void AppendStringTag(Byte[] name)
		{
			if(activeTag is not CompoundTag tag)
			{
				throw new MalformedNbtException("Named string tag discovered outside of a CompoundTag");
			}

			Span<Byte> buffer = stackalloc Byte[2], array;

			stream.Read(buffer);

			nameLength = BinaryPrimitives.ReadInt16BigEndian(buffer); // we reuse nameLength for the string length
			array = new Byte[nameLength];

			stream.Read(array);

			tag.Children.Add(new StringTag(name, array));
		}

		void AppendCompoundTag(Byte[] name)
		{
			CompoundTag compound = new(name, new());
			compound.Parent = activeTag;

			if(activeTag is not CompoundTag tag)
			{
				throw new MalformedNbtException("Named compound tag discovered outside of a CompoundTag");
			}

			tag.Children.Add(compound);

			activeTag = compound;

			depth++;
		}
	}

	#region Reading primitives

	/// <summary>
	/// Reads one <see cref="ByteTag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public ByteTag ReadByteTag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[1];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), payload[0]);
	}

	/// <summary>
	/// Reads one <see cref="Int16Tag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public Int16Tag ReadInt16Tag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[2];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), BinaryPrimitives.ReadInt16BigEndian(payload));
	}

	/// <summary>
	/// Reads one <see cref="Int32Tag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public Int32Tag ReadInt32Tag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[4];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), BinaryPrimitives.ReadInt32BigEndian(payload));
	}

	/// <summary>
	/// Reads one <see cref="Int64Tag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public Int64Tag ReadInt64Tag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[8];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), BinaryPrimitives.ReadInt64BigEndian(payload));
	}

	/// <summary>
	/// Reads one <see cref="FloatTag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public FloatTag ReadFloatTag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[4];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), BinaryPrimitives.ReadSingleBigEndian(payload));
	}

	/// <summary>
	/// Reads one <see cref="DoubleTag"/> from the current stream.
	/// </summary>
	/// <param name="stream">A stream containing valid NBT data.</param>
	public DoubleTag ReadDoubleTag(Stream stream)
	{
		Span<Byte> name = stackalloc Byte[0], nameLengthBuffer = stackalloc Byte[2], payload = stackalloc Byte[8];
		Int16 nameLength;

		stream.Read(nameLengthBuffer);
		nameLength = BinaryPrimitives.ReadInt16BigEndian(nameLengthBuffer);

		name = stackalloc Byte[nameLength];

		stream.Read(name);
		stream.Read(payload);

		return new(name.ToArray(), BinaryPrimitives.ReadDoubleBigEndian(payload));
	}

	#endregion
}
