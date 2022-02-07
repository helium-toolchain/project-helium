namespace Helium.Data.Nbt.Serialization;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

/// <summary>
/// Contains methods for throwing exceptions from the performance-critical binary reader and traverser.
/// </summary>
internal static class ThrowHelper
{
	internal static void ThrowInvalidListTokenType(NbtTokenType type)
	{
		throw new ArgumentException($"List of invalid type {type} encountered.");
	}

	internal static void ThrowMaximumDepthExceeded(Int32 depth, Int32 maxDepth)
	{
		throw new ArgumentException($"The passed NBT data exceeded the maximum depth of {maxDepth} with a depth of {depth}.");
	}

	internal static void ThrowStringifiedNotWrappedIntoCompound()
	{
		throw new ArgumentException("The passed sNBT data was not wrapped into a root compound.");
	}

	internal static void ThrowStringifiedUnquotedStringContainsQuotations()
	{
		throw new ArgumentException("The passed sNBT data contains a quotation mark inside an unquoted string.");
	}

	internal static void ThrowStringifiedIdentifierNotFollowedByColon()
	{
		throw new ArgumentException("The passed sNBT data contains an identifier not followed by a colon.");
	}

	[RequiresPreviewFeatures]
	internal static IDataToken ThrowStringifiedInvalidArrayIdentifier()
	{
		throw new ArgumentException("The passed sNBT data contains an array with invalid type identifier.");
	}

	internal static void ThrowInvalidIntegerArrayElement(String type)
	{
		throw new ArgumentException($"The passed sNBT data contains an array of type {type} with an invalid value.");
	}

	internal static void ThrowStringifiedInvalidIntegerArray()
	{
		throw new ArgumentException("The passed sNBT contains an invalid array token.");
	}
}
