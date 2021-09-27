namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-S0x01: Serverbound login flow start packet.
/// </summary>
public class LoginStartPacket : IPacket
{
	public VarInt Id => 0x00;

	/// <summary>
	/// Player username
	/// </summary>
	public String Username { get; set; }

	public void Read(PacketStream stream)
	{
		Username = stream.ReadString(16);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + Username.Length + 1); // again, we know the VarInt length will never exceed 1
		stream.WriteVarInt(Id);
		stream.WriteString(Username);
	}
}
