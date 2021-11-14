namespace Helium.Nbt;

using System;
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
}
