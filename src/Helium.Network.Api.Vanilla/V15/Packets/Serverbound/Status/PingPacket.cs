namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Status;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla;

using System;

/// <summary>
/// Status-S0x01: Serverbound Ping answer packet, identifier 0x01
/// </summary>
public class PingPacket : IPacket
{
	public VarInt Id => 0x01;

	/// <summary>
	/// Int64 payload; on mojang clients this is a system-dependent time value used to calculate a rough server ping approximation.
	/// </summary>
	public Int64 Payload { get; set; }

	public void Read(PacketStream stream)
	{
		this.Payload = stream.ReadInt64();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + 8);
		stream.WriteVarInt(Id);
		stream.WriteInt64(Payload);
	}
}
