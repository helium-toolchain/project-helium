﻿namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Status;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla;

/// <summary>
/// Status-C0x01: Clientbound Ping answer packet, identifier 0x01
/// </summary>
public struct PingAnswerPacket : IPacket
{
	public VarInt Id => 0x01;

	/// <summary>
	/// Int64 payload; should be the same as sent by the client in S0x01
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
