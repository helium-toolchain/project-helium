namespace Helium.Network.Api.Vanilla.V15;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

/// <summary>
/// Holds data for a block change; used by <see cref="MultiBlockChangePacket"/>
/// </summary>
public record struct BlockChangeData
{
	/// <summary>
	/// Encodes the horizontal position in the given chunk.
	/// </summary>
	/// <remarks>
	/// The four most significant bits encode the X coordinate, the four least significant bits encode the Z coordinate.
	/// </remarks>
	public Byte HorizontalPosition { get; set; }

	/// <summary>
	/// The Y coordinate in the given chunk.
	/// </summary>
	public Byte YCoordinate { get; set; }

	/// <summary>
	/// The new blockstate at the given coordinates.
	/// </summary>
	public VarInt Blockstate { get; set; }
}
