namespace Helium.Api.Mojang;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

/// <summary>
/// VarLong type, for semi-native handling of mojangs usage of VarLong
/// </summary>
public struct VarLong
{
	private Int64 __value;

	public static implicit operator Int64 (VarLong val)
	{
		return val.__value;
	}

	public static implicit operator VarLong (Int64 val)
	{
		return new VarLong { __value = val };
	}

	/// <summary>
	/// Reads a VarLong from up to 10 bytes from the stream. The stream will advance to the start of the next element.
	/// </summary>
	/// <param name="stream">A MemoryStream starting with the first byte of the VarInt.</param>
	public void Read(MemoryStream stream)
	{
		Int64 result = 0;
		Int32 tv;
		Byte current, readCounter = 0;

		do
		{
			current = (Byte)stream.ReadByte();

			tv = current & 0b0111_1111;
			result |= (UInt16)(tv << (7 * readCounter++));

			if (readCounter > 10)
			{
				throw new InvalidOperationException("The specified VarLong is too big");
			}
		} while ((current & 0b1000_0000) != 0);

		__value = result;
	}

	/// <summary>
	/// Writes a VarLong of up to 10 bytes to the stream.
	/// </summary>
	/// <param name="stream">The MemoryStream the VarLong will be appended to.</param>
	public void Write(MemoryStream stream)
	{
		UInt64 unsigned = (UInt64)__value;
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

	/// <summary>
	/// Gets the length of this VarLong were it serialized.
	/// </summary>
	public Byte Length
	{
		get
		{
			UInt64 unsigned = (UInt64)__value;
			Byte length = 0;

			do
			{
				unsigned >>= 7;

				length++;
			} while (unsigned != 0);

			return length;
		}
	}
}
