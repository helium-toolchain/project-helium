namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-S0x02: Login plugin response packet. Probably unused by Helium.
/// Requires the packet length to be passed via constructor for the read method.
/// </summary>
public class LoginPluginResponsePacket : IPacket
{
	public Int32 PacketLength { get; private set; }

	public VarInt Id => 0x02;

	/// <summary>
	/// Message ID this client is responding to.
	/// </summary>
	public VarInt MessageId { get; set; }

	/// <summary>
	/// Indicates whether the client could interpret the payload. If <see langword="false"/>, no payload follows.
	/// </summary>
	public Boolean Success { get; set; }

	/// <summary>
	/// Additional client response payload.
	/// </summary>
	public Byte[] AdditionalData { get; set; }

	public void Read(PacketStream stream)
	{
		MessageId = stream.ReadVarInt();
		Success = stream.ReadBoolean();
		AdditionalData = stream.ReadByteArray(PacketLength - (Id.Length + MessageId.Length + 1));
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + MessageId.Length + 1 + AdditionalData.Length);
		stream.WriteVarInt(Id);
		stream.WriteVarInt(MessageId);
		stream.WriteUnsignedByte((Byte)(Success ? 0x01b : 0x00b));
		stream.WriteByteArray(AdditionalData);
	}

	public LoginPluginResponsePacket(Int32 packetLength)
	{
		this.PacketLength = packetLength;
	}
}
