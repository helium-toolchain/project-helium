namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-C0x00: Clientbound disconnect-on-login packet
/// </summary>
public struct LoginDisconnectPacket : IPacket
{
	public VarInt Id => 0x00;

	/// <summary>
	/// Disconnect reason - may be whitelist, ban or anything similar.
	/// </summary>
	public String Reason { get; set; }

	public void Read(PacketStream stream)
	{
		Reason = stream.ReadString(262144); // chat can never exceed this length. This limit does not apply to Helium networking.
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + ((VarInt)Reason.Length).Length + Reason.Length);
		stream.WriteVarInt(Id);
		stream.WriteString(Reason);
	}
}
