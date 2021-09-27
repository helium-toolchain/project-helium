namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-C0x02: Clientbound login success packet
/// </summary>
internal class LoginSuccessPacket : IPacket
{
	public VarInt Id => 0x02;

	/// <summary>
	/// UUID but with hyphens. Why is mojang like this...
	/// </summary>
	public String Uuid { get; set; }

	/// <summary>
	/// Username of the client we're sending this packet to. 
	/// </summary>
	public String Username { get; set; }

	public void Read(PacketStream stream)
	{
		Uuid = stream.ReadString(36);
		Username = stream.ReadString(16);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + Uuid.Length + Username.Length + 2); // we dont ever have username/uuid VarInt lengths > 1 byte
		stream.WriteVarInt(Id);
		stream.WriteString(Uuid);
		stream.WriteString(Username);
	}
}
