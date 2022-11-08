namespace Helium.Network;

using System;
using System.Threading.Tasks;

using Helium.Network.Results;

/// <summary>
/// Represents a packet transformer, capable of applying any transformations to a given packet.
/// </summary>
public interface IPacketTransformer
{
	/// <summary>
	/// Transforms a serialized clientbound packet.
	/// </summary>
	/// <param name="packet">The serialized value of this packet.</param>
	/// <returns>A result indicating whether the operation was successful.</returns>
	public ValueTask<NetworkResult> TransformClientboundSerialized
	(
		ref Memory<Byte> packet
	);

	/// <summary>
	/// Transforms a deserialized clientbound packet.
	/// </summary>
	/// <param name="packet">The packet to transform.</param>
	/// <returns>A result indicating whether the operation was successful.</returns>
	public NetworkResult TransformClientbound
	(
		ref IPacket packet
	);

	/// <summary>
	/// Transforms a serialized serverbound packet.
	/// </summary>
	/// <param name="packet">The serialized value of this packet.</param>
	/// <returns>A result indicating whether the operation was successful.</returns>
	public ValueTask<NetworkResult> TransformServerboundSerialized
	(
		ref Memory<Byte> packet
	);

	/// <summary>
	/// Transforms a deserialized serverbound packet.
	/// </summary>
	/// <param name="packet">The packet to transform.</param>
	/// <returns>A result indicating whether the operation was successful.</returns>
	public NetworkResult TransformServerbound
	(
		ref IPacket packet
	);
}
