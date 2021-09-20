namespace Helium.Api.Mojang.Entity;

using System;

/// <summary>
/// Base Entity Metadata class. More precise information and tooling is found in Helium.Network.Api.Vanilla.
/// </summary>
public class MojangEntityMetadata
{
	/// <summary>
	/// Index of the metadata field. Unique for the entire array, if this is 0xFF the array ends (because a length declaration would be too hard)
	/// </summary>
	public Byte Index { get; init; }

	/// <summary>
	/// Type of the metadata field. More information and tooling is found in Helium.Network.Api.Vanilla.
	/// </summary>
	public VarInt Type { get; init; }

	/// <summary>
	/// Value of the metadata field, as required by <see cref="Type"/>. More information if sound in Helium.Network.Api.Vanilla.
	/// </summary>
	public Object Value { get; init; }
}
