namespace Helium.Data.Castle.Serialization;

using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

/// <summary>
/// Offers ways to convert to and from binary Castle data.
/// </summary>
/// <remarks>
/// Since Castle does not use compression locally, there are no compression methods offered. If needed, compression should
/// be handled by the consumer.
/// </remarks>
public static class CastleConverter
{
	/// <summary>
	/// Reads a CastleRootToken from a span. This does not guarantee validity of the returned Castle token, neither does it check
	/// validity of the input span.
	/// </summary>
	/// <param name="blob">The input span. Validity of this span is not guaranteed.</param>
	/// <returns>A <see cref="CastleRootToken"/> read from the input blob. Validity is not guaranteed.</returns>
	[RequiresPreviewFeatures]
	public static CastleRootToken Deserialize(Span<Byte> blob)
	{
		return new BinaryCastleReader(blob).Deserialize();
	}

	/// <summary>
	/// Reads a CastleRootToken from a byte array. This does not guarantee validity of the returned Castle token, neither does it
	/// check validity of the input.
	/// </summary>
	/// <param name="blob">The input byte array. Validity of this byte array is not guaranteed.</param>
	/// <returns>A <see cref="CastleRootToken"/> read from the input blob. Validity is not guaranteed.</returns>
	[RequiresPreviewFeatures]
	public static CastleRootToken Deserialize(Byte[] blob)
	{
		return new BinaryCastleReader(blob.AsSpan()).Deserialize();
	}

	/// <summary>
	/// Reads a CastleRootToken from a UTF8 string. This does not guarantee validity of the returned Castle token, neither does it
	/// check validity of the input.
	/// </summary>
	/// <param name="blob">The input string. Validity of this string is not guaranteed.</param>
	/// <returns>A <see cref="CastleRootToken"/> read from the input blob. Validity is not guaranteed.</returns>
	[RequiresPreviewFeatures]
	public static CastleRootToken Deserialize(String blob)
	{
		return new BinaryCastleReader(Encoding.UTF8.GetBytes(blob).AsSpan()).Deserialize();
	}

	/// <summary>
	/// Reads a CastleRootToken from a MemoryStream. This does not guarantee validity of the returned Castle token, neither does it
	/// check validity of the input.
	/// </summary>
	/// <param name="blob">The input <see cref="MemoryStream"/>. Validity is not guaranteed.</param>
	/// <returns>A <see cref="CastleRootToken"/> read from the input blob. Validity is not guaranteed.</returns>
	[RequiresPreviewFeatures]
	public static CastleRootToken Deserialize(MemoryStream stream)
	{
		return new BinaryCastleReader(stream.ToArray().AsSpan()).Deserialize();
	}

	/// <summary>
	/// Writes a CastleRootToken to a span. This does not or only partially check validity of the input.
	/// </summary>
	/// <param name="root">The input <see cref="CastleRootToken"/>. Validity is not guaranteed.</param>
	/// <returns>A <see cref="Span{Byte}"/> written based on the input token.</returns>
	[RequiresPreviewFeatures]
	public static Span<Byte> Serialize(CastleRootToken root)
	{
		BinaryCastleWriter writer = new(root);
		writer.Serialize();
		return writer.Buffer;
	}

	/// <summary>
	/// Writes a CastleRootToken to a span. This does not or only partially check validity of the input.
	/// </summary>
	/// <param name="root">The input <see cref="CastleRootToken"/>. Validity is not guaranteed.</param>
	/// <param name="resizeIncrement">The amount of bytes the buffer should be prolonged by when it is resized.
	/// Defaults to the platform memory page default size, or 1024 if Helium does not know the current platform.</param>
	/// <returns>A <see cref="Span{Byte}"/> written based on the input token.</returns>
	[RequiresPreviewFeatures]
	public static Span<Byte> Serialize(CastleRootToken root, Int32 resizeIncrement)
	{
		BinaryCastleWriter writer = new(root, resizeIncrement);
		writer.Serialize();
		return writer.Buffer;
	}
}
