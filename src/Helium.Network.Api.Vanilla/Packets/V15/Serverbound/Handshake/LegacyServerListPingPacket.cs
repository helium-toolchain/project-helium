namespace Helium.Network.Api.Vanilla.Packets.V15.Serverbound.Handshake;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Legacy server-list ping packet. Pre-1.7.2 clients can still send this packet and the server should handle it correctly.
/// </summary>
public class LegacyServerListPingPacket : ILegacyPacket
{
	/// <summary>
	/// Packet Identifier.
	/// </summary>
	[SuppressMessage("Design", "CA1822")]
	public Byte Id => 0xFE;

	/// <summary>
	/// Packet payload. This is always 0x01.
	/// </summary>
	public Byte Payload { get; set; } = 0x01;

	public void Read(PacketStream stream)
	{
		_ = stream.ReadUnsignedByte();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteUnsignedByte(Id);
		stream.WriteUnsignedByte(Payload);
	}
}
