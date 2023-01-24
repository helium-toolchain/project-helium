namespace Helium.Networking.Api.Vanilla.IO;

using System;
using System.Runtime.CompilerServices;

[SkipLocalsInit]
public unsafe static class LEB128Marshal
{
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static UInt32 encodeLEB128
	(
		UInt64 value,
		Byte* target
	)
	{
		Byte* p = target;

		do
		{
			Byte b = (Byte)(value & 0x7F);
			value >>= 7;

			b |= 0x80;

			*p++ = b;
		} while (value != 0);

		*p &= 0x7F;

		return (UInt32)(p - target);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static UInt64 decodeLEB128
	(
		Byte* origin,
		ref UInt32 length
	)
	{
		Byte* p = origin;
		UInt64 value = 0;
		Int32 shift = 0;

		do
		{
			Int32 slice = *p & 0x7F;
			value += (UInt64)(slice << shift);
			shift += 7;
		} while (*p++ >= 128);

		length = (UInt32)(origin - p);

		return value;
	}
}
