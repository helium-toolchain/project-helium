namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x09: Clientbound block mining animation packet.
/// </summary>
public struct BlockMineAnimationPacket : IPacket
{
	public VarInt Id => 0x09;

	/// <summary>
	/// Entity ID of the entity breaking this block. This entity does not need to actually exist...
	/// </summary>
	public VarInt EntityId { get; set; }

	/// <summary>
	/// Location of the block in question.
	/// </summary>
	public Position Location { get; set; }

	/// <summary>
	/// Destroy stage: 0-9 to set the animation stage, any other value to remove it.
	/// </summary>
	public Byte Stage { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityId = stream.ReadVarInt();
		this.Location = stream.ReadPosition();
		this.Stage = stream.ReadUnsignedByte();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityId.Length + 9);
		stream.WriteVarInt(this.EntityId);
		stream.WritePosition(this.Location);
		stream.WriteUnsignedByte(this.Stage);
	}
}
