namespace Helium.Data.Nbt;

using System;
using System.IO;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides some useful extensions to <see cref="MemoryStream"/>.
/// </summary>
public static class MemoryStreamExtensions
{
	/// <summary>
	/// Skips the specified amount of bytes from being read.
	/// </summary>
	public static void Skip(this MemoryStream stream, Int32 count)
	{
		stream.Position += count;
	}

	/// <summary>
	/// Peeks at the next byte in the stream.
	/// </summary>
	public static Byte Peek(this MemoryStream stream)
	{
		Byte v = (Byte)stream.ReadByte();
		stream.Position--;
		return v;
	}

	/// <summary>
	/// Peeks at the specified byte in the stream.
	/// </summary>
	/// <param name="count">The one-based offset from the current stream position.</param>
	public static Byte Peek(this MemoryStream stream, Int32 count)
	{
		stream.Skip(count - 1);
		Byte v = (Byte)stream.ReadByte();
		stream.Position -= count;
		return v;
	}

	/// <summary>
	/// Skips all whitespace characters leading the stream.
	/// </summary>
	public static void SkipWhitespace(this MemoryStream stream)
	{
		while(stream.Peek() == 0x20)
		{
			stream.Position++;
		}
	}

	/// <summary>
	/// Returns whether the next non-whitespace character matches the specified character.
	/// </summary>
	public static Boolean Expect(this MemoryStream stream, Byte value)
	{
		stream.SkipWhitespace();

		if(stream.Peek() == value)
		{
			stream.Position++;
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Checks whether the specified byte is an alphabetical letter.
	/// </summary>
	public static Boolean IsLetter(this Byte value)
	{
		return (value < 0x7B && value > 0x60) || (value < 0x5B && value > 0x40);
	}

	/// <summary>
	/// Checks whether the specified byte is a numerical letter.
	/// </summary>
	public static Boolean IsNumber(this Byte value)
	{
		return value < 0x3A && value > 0x2F;
	}

	/// <summary>
	/// Checks whether the specified byte is an alphanumeric character.
	/// </summary>
	public static Boolean IsAlphanumeric(this Byte value)
	{
		return value.IsLetter() || value.IsNumber();
	}

	/// <summary>
	/// Reads a specified amount of bytes to an array.
	/// </summary>
	public static Byte[] ReadBytes(this MemoryStream source, Int32 count)
	{
		Span<Byte> buffer = stackalloc Byte[count];
		source.Read(buffer);
		return buffer.ToArray();
	}
}