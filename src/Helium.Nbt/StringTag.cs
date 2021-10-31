namespace Helium.Nbt;

using System;
using System.Runtime.Versioning;
using System.Text;

/// <summary>
/// Represents an UTF-8 string tag, prefixed with an unsigned 16-bit integer.
/// </summary>
[RequiresPreviewFeatures]
public sealed class StringTag : NbtTag
{
	public static new Byte Declarator => 0x08;

	public String Content { get; private set; }

	public StringTag(Byte[] name, Span<Byte> value)
	{
		this.Name = name;
		this.Content = Encoding.UTF8.GetString(value);
	}
}
