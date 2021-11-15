namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 16-bit integer token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtInt16Token : IValuedNbtToken<Int16>
{
	public static Byte Declarator => 0x02;

	public static Int32 Length => 2;

	public Int16 Value { get; init; }

	public Byte[] Name { get; init; }

	public NbtInt16Token(Byte[] name, Int16 value)
	{
		this.Name = name;
		this.Value = value;
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtInt16Token t = (NbtInt16Token)token;
		Span<Byte> buffer = stackalloc Byte[2];

		BinaryPrimitives.WriteInt16BigEndian(buffer, t.Value);

		stream.Write(buffer);
	}
}
