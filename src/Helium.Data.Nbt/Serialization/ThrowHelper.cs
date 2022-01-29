namespace Helium.Data.Nbt.Serialization;

using System;

/// <summary>
/// Contains methods for throwing exceptions from the performance-critical binary reader and traverser.
/// </summary>
internal static class ThrowHelper
{
	internal static void ThrowInvalidListTokenType(NbtTokenType type)
	{
		throw new ArgumentException($"List of type {type} encountered.");
	}

	internal static void ThrowMaximumDepthExceeded(Int32 depth, Int32 maxDepth)
	{
		throw new ArgumentException($"The passed NBT data exceeded the maximum depth of {maxDepth} with a depth of {depth}");
	}
}
