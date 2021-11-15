namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Handshake;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla;

public struct HandshakePacket : IPacket
{
	public VarInt Id => 0x00;

	/// <summary>
	/// Protocol version used to connect to the server. 573 for 1.15, 575 for 1.15.1, 578 for 1.15.2
	/// </summary>
	public VarInt ProtocolVersion { get; set; }

	/// <summary>
	/// State the websocket connection should switch to.
	/// 1 for status, 2 for login.
	/// </summary>
	public VarInt NextState { get; set; }

	/// <summary>
	/// Port number this packet was sent to.
	/// </summary>
	public UInt16 Port { get; set; } = 25565;

	/// <summary>
	/// 255-char string of the address, either hostname or IP, this packet was sent to.
	/// </summary>
	public String Address { get; set; }

	public void Read(PacketStream stream)
	{
		ProtocolVersion = stream.ReadVarInt();
		Address = stream.ReadString();
		Port = stream.ReadUInt16();
		NextState = stream.ReadVarInt();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + ProtocolVersion.Length + NextState.Length + 4 + ((VarInt)Address.Length).Length + Address.Length);
		stream.WriteVarInt(Id);
		stream.WriteVarInt(ProtocolVersion);
		stream.WriteString(Address);
		stream.WriteUInt16(Port);
		stream.WriteVarInt(NextState);
	}
}
