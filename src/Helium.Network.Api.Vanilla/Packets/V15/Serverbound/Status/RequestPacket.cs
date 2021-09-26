﻿namespace Helium.Network.Api.Vanilla.Packets.V15.Serverbound.Status;

using Helium.Api.Mojang;

/// <summary>
/// Status-S0x00: Serverbound request initiation packet
/// </summary>
public class RequestPacket : IPacket
{
	public VarInt Id => 0x00;

	public void Read(PacketStream stream)
	{
		return;
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length);
		stream.WriteVarInt(Id);
	}
}
