namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Entities;

/// <summary>
/// C0x06: Clientbound Entity Animation, see <see cref="EntityAnimations"/>
/// </summary>
public struct EntityAnimationPacket : IPacket
{
	public VarInt Id => 0x06;

	/// <summary>
	/// Server-side Entity ID of the player in question. Can in theory be sent for non-player entities, though that would be undefined behaviour.
	/// </summary>
	public VarInt EntityID { get; set; }

	/// <summary>
	/// Animation, see <see cref="EntityAnimations"/>
	/// </summary>
	public Byte Animation { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityID = stream.ReadVarInt();
		this.Animation = stream.ReadUnsignedByte();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityID.Length + 1);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityID);
		stream.WriteByte(Animation);
	}
}

