namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 32-bit integer token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtInt32Token : IValuedNbtToken<Int32>
{
	public static Byte Declarator => 0x03;

	public static Int32 Length => 4;

	public Int32 Value { get; init; }

	public Byte[] Name { get; init; }

	public NbtInt32Token(Byte[] name, Int32 value)
	{
		this.Name = name;
		this.Value = value;
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtInt32Token t = (NbtInt32Token)token;
		Span<Byte> buffer = stackalloc Byte[4];

		BinaryPrimitives.WriteInt32BigEndian(buffer, t.Value);

		stream.Write(buffer);
	}
}
