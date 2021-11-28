namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a signed, big endian 64-bit integer token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtInt64Token : IValuedNbtToken<Int64>
{
	public static Byte Declarator => 0x04;

	public static Int32 Length => 8;

	public Int64 Value { get; init; }

	public Byte[] Name { get; init; }

	public INbtToken? Parent { get; set; } = null;

	public NbtInt64Token(Byte[] name, Int64 value)
	{
		this.Name = name;
		this.Value = value;
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtInt64Token t = (NbtInt64Token)token;
		Span<Byte> buffer = stackalloc Byte[8];

		BinaryPrimitives.WriteInt64BigEndian(buffer, t.Value);

		stream.Write(buffer);
	}
}
