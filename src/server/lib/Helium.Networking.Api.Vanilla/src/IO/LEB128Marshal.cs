namespace Helium.Networking.Api.Vanilla.IO;

using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// Contains methods to marshal LEB-128-encoded integers as utilized throughout Minecraft's networking protocol.
/// </summary>
[SkipLocalsInit]
public unsafe static class LEB128Marshal
{
	/// <summary>
	/// Encodes the given integer as LEB-128.
	/// </summary>
	/// <remarks>
	/// This treats all integers as unsigned integers, meaning that it is only compatible with signed integers
	/// by reinterpret-casting after decoding again.
	/// </remarks>
	/// <typeparam name="T">
	/// The integer type to encode. An exception will be thrown on invocation if this integer is larger than UInt64.
	/// </typeparam>
	/// <param name="buffer">The buffer to encode into. Up to 10 bytes will be populated.</param>
	/// <param name="value">The value to encode.</param>
	/// <returns>The amount of bytes the encoding operation populated.</returns>
	public static UInt32 Encode<T>
	(
		Span<Byte> buffer,
		T value
	)
		where T : unmanaged, IBinaryInteger<T>, IMinMaxValue<T>
	{
		ValidationAndThrowHelper.ThrowIfIntegerExceedsSizeLimit<T>();

		fixed(Byte* origin = buffer)
		{
			return encodeLEB128
			(
				*(UInt64*)&value,
				origin
			);
		}
	}

	/// <summary>
	/// Decodes a LEB-128 integer from the given buffer.
	/// </summary>
	/// <remarks>
	/// This treats all data as unsigned LEB-128, meaning that it is only compatible with signed integers
	/// by reinterpret-casting before encoding.
	/// </remarks>
	/// <typeparam name="T">
	/// The integer type to decode. An exception will be thrown on invocation if this integer is larger than UInt64.
	/// </typeparam>
	/// <param name="buffer">The buffer to read from. Up to 10 bytes will be consumed.</param>
	/// <param name="value">The decoded value.</param>
	/// <returns>The amount of bytes that have been read.</returns>
	public static UInt32 Decode<T>
	(
		Span<Byte> buffer,
		out T value
	)
		where T : unmanaged, IBinaryInteger<T>, IMinMaxValue<T>
	{
		ValidationAndThrowHelper.ThrowIfIntegerExceedsSizeLimit<T>();

		value = T.Zero;

		fixed(Byte* origin = buffer)
		{
			return decodeLEB128
			(
				origin,
				ref Unsafe.As<T, UInt64>(ref value)
			);
		}
	}

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
	private static UInt32 decodeLEB128
	(
		Byte* origin,
		ref UInt64 value
	)
	{
		Byte* p = origin;
		Int32 shift = 0;

		do
		{
			Int32 slice = *p & 0x7F;
			value += (UInt64)(slice << shift);
			shift += 7;
		} while (*p++ >= 128);

		return (UInt32)(origin - p);
	}
}

file static class ValidationAndThrowHelper
{
	private static UInt64 container = UInt64.MaxValue;

	[StackTraceHidden]
	public static void ThrowIfIntegerExceedsSizeLimit<T>()
		where T : IBinaryInteger<T>, IMinMaxValue<T>
	{
		if(T.MaxValue > Unsafe.As<UInt64, T>(ref container))
		{
			throw new ArgumentException
			(
				"The specified integer type is too large to be marshalled to LEB-128 in concordance with " +
				"Minecraft's protocol rules."
			);
		}
	}
}
