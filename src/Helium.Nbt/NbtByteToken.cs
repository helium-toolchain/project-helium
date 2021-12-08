namespace Helium.Nbt;

using System;
using System.IO;
using System.Runtime.Versioning;

/// <summary>
/// Represents a NBT Byte Token
/// </summary>
[RequiresPreviewFeatures]
public record struct NbtByteToken : IValuedNbtToken<SByte>
{
	public static Byte Declarator => 0x01;

	public static Int32 Length => 1;

	public Byte[] Name { get; init; }

	public SByte Value { get; init; }
	public INbtToken? Parent { get; set; } = null;

	public NbtByteToken(Byte[] name, SByte value)
	{
		this.Name = name;
		this.Value = value;
	}

	public static void WriteNameless(Stream stream, INbtToken token)
	{
		NbtByteToken t = (NbtByteToken)token;
		stream.WriteByte((Byte)t.Value);
	}
}
