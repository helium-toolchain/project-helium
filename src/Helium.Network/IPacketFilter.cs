namespace Helium.Network;

using System;

/// <summary>
/// Represents a type capable of blocking packets from being sent or received.
/// </summary>
public interface IPacketFilter
{
	/// <summary>
	/// Indicates whether the server should process this packet.
	/// </summary>
	/// <param name="packet">The deserialized packet.</param>
	public Boolean ShouldReceive
	(
		IPacket packet
	);

	/// <summary>
	/// Indicates whether the server should send this packet.
	/// </summary>
	/// <param name="packet">The not-yet-serialized packet.</param>
	public Boolean ShouldSend
	(
		IPacket packet
	);
}
