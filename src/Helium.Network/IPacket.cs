namespace Helium.Network;

using System;

/// <summary>
/// Represents a basic packet.
/// </summary>
public interface IPacket
{
	/// <summary>
	/// The protocol ID of this packet.
	/// </summary>
	public Int32 Id { get; set; }

	/// <summary>
	/// The protocol this packet belongs to.
	/// </summary>
	public static abstract String ProtocolId { get; set; }

	/// <summary>
	/// The protocol version this packet belongs to.
	/// </summary>
	public static abstract Int32 ProtocolVersion { get; set; }	
}
