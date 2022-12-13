namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x0B: Perform Block Action packet
/// </summary>
public struct BlockActionPacket : IPacket
{
	public VarInt Id => 0x0B;

	/// <summary>
	/// Describes the location of the block in question.
	/// </summary>
	public Position Location { get; set; }

	/// <summary>
	/// The ID of this action.
	/// </summary>
	public Byte ActionId { get; set; }

	/// <summary>
	/// The singular parameter for this action.
	/// </summary>
	public Byte ActionParameter { get; set; }

	/// <summary>
	/// The registry block ID.
	/// </summary>
	public VarInt BlockType { get; set; }

	public void Read(PacketStream stream)
	{
		this.Location = stream.ReadPosition();
		this.ActionId = stream.ReadUnsignedByte();
		this.ActionParameter = stream.ReadUnsignedByte();
		this.BlockType = stream.ReadVarInt();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.BlockType.Length + 10);
		stream.WriteVarInt(this.Id);
		stream.WritePosition(this.Location);
		stream.WriteUnsignedByte(this.ActionId);
		stream.WriteUnsignedByte(this.ActionParameter);
		stream.WriteVarInt(this.BlockType);
	}
}
