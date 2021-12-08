namespace Helium.Nbt;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

/// <summary>
/// Represents an UTF-8 string token, prefixed with an unsigned 16-bit integer.
/// </summary>
[RequiresPreviewFeatures]
public sealed class NbtStringToken : IValuedNbtToken<String>
{
	public static Byte Declarator => 0x08;

	public String Value { get; set; }

	public static Int32 Length => 0;

	public Byte[] Name { get; set; }

	public NbtStringToken(Byte[] name, Span<Byte> value)
	{
		this.Name = name;
		this.Value = Encoding.UTF8.GetString(value);
	}

	public NbtStringToken(Byte[] name, String value)
	{
		this.Name = name;
		this.Value = value;
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtStringToken t = (NbtStringToken)token;
		Span<Byte> buffer = stackalloc Byte[2];

		BinaryPrimitives.WriteInt16BigEndian(buffer, (Int16)t.Value.Length);

		stream.Write(buffer);
		stream.Write(Encoding.UTF8.GetBytes(t.Value));
	}
}
