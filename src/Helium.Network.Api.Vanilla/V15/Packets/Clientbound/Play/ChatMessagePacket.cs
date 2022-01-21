namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x0F - Clientbound Chat Message packet
/// </summary>
public struct ChatMessagePacket : IPacket
{
	public VarInt Id => 0x0F;

	/// <summary>
	/// The message contents.
	/// </summary>
	public String Message { get; set; }

	/// <summary>
	/// Type of the message. 0 -> chat message; 1 -> system message; 2 -> game info message, rendered above the hotbar.
	/// </summary>
	public Byte Type { get; set; }

	public void Read(PacketStream stream)
	{
		this.Message = stream.ReadString();
		this.Type = stream.ReadUnsignedByte();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + ((VarInt)this.Message.Length).Length + this.Message.Length + 1);
		stream.WriteVarInt(this.Id);
		stream.WriteString(this.Message);
		stream.WriteUnsignedByte(this.Type);
	}
}
