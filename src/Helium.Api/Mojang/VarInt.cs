namespace Helium.Api.Mojang;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

/// <summary>
/// VarInt type, for semi-native handling of mojangs usage of VarInt
/// </summary>
public struct VarInt
{
	private Int32 __value;

	public static implicit operator Int32 (VarInt var)
	{
		return var.__value;
	}

	public static implicit operator VarInt (Int32 var)
	{
		return new VarInt { __value = var };
	}

	/// <summary>
	/// Reads a VarInt from up to 5 bytes from the stream. The stream will advance to the start of the next element.
	/// </summary>
	/// <param name="stream">A MemoryStream starting with the first byte of the VarInt.</param>
	
	[SuppressMessage("Reliability", "CA2014", Justification = "The loop can only run five times, no stackoverflow will happen here")]
	public void Read(MemoryStream stream)
	{
		Int32 readCounter = 0, result = 0, tv = 0;
		Byte current;

		do
		{
			Span<Byte> buffer = stackalloc Byte[1];
			stream.Read(buffer);

			current = buffer[0];

			tv = current & 0b0111_1111;
			result |= tv << (7 * readCounter++);

			if (readCounter > 5)
			{
				throw new InvalidOperationException("The specified VarInt is too big");
			}
		} while ((current & 0b1000_0000) != 0);

		__value = result;
	}

	/// <summary>
	/// Writes a VarInt of up to 5 bytes to the span.
	/// </summary>
	/// <param name="stream">The MemoryStream the VarInt will be appended to.</param>
	public void Write(MemoryStream stream)
	{
		UInt32 unsigned = (UInt32)__value;
		Byte temp;

		do
		{
			temp = (Byte)(unsigned & 127);
			unsigned >>= 7;

			if (unsigned != 0)
			{
				temp |= 128;
			}

			stream.WriteByte(temp);
		} while (unsigned != 0);
	}
}
