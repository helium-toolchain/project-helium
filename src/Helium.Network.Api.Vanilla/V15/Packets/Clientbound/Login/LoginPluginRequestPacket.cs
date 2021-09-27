namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-C0x04: Login plugin request packet. Probably unused by Helium?
/// Requires the packet length to be passed via constructor for the read method.
/// </summary>
public class LoginPluginRequestPacket : IPacket
{
	/// <summary>
	/// Length of this packet, see <see cref="LoginPluginRequestPacket"/> documentation.
	/// </summary>
	public Int32 PacketLength { get; private set; }

	public VarInt Id => 0x04;

	/// <summary>
	/// Server-generated ID unique to this connection.
	/// </summary>
	public VarInt MessageId { get; set; }

	/// <summary>
	/// Plugin channel identifier used.
	/// </summary>
	public String PluginChannel { get; set; }

	/// <summary>
	/// Any remaining data sent via this packet.
	/// </summary>
	public Byte[] RemainingData { get; set; }

	public void Read(PacketStream stream)
	{
		MessageId = stream.ReadVarInt();
		PluginChannel = stream.ReadString();
		RemainingData = stream.ReadByteArray(MessageId.Length + ((VarInt)PluginChannel.Length).Length + PluginChannel.Length);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + MessageId.Length + ((VarInt)PluginChannel.Length).Length + PluginChannel.Length + RemainingData.Length);
		stream.WriteVarInt(Id);
		stream.WriteVarInt(MessageId);
		stream.WriteString(PluginChannel);
		stream.WriteByteArray(RemainingData);
	}

	public LoginPluginRequestPacket(Int32 packetLength)
	{
		this.PacketLength = packetLength;
	}
}
