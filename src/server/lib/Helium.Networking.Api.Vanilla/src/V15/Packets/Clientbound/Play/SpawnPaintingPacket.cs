namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Entities;

/// <summary>
/// C0x04: Spawn painting packet.
/// </summary>
public struct SpawnPaintingPacket : IPacket
{
	public VarInt Id => 0x04;

	/// <summary>
	/// Server-side entity ID of this painting.
	/// </summary>
	public VarInt EntityID { get; set; }

	/// <summary>
	/// Packet motive, defining its size and client rendering. Matches <see cref="Paintings"/>.
	/// </summary>
	public VarInt Motive { get; set; }

	/// <summary>
	/// Center coordinates of this painting.
	/// </summary>
	/// <remarks>
	/// Center algorithm: given a <c>(width × height)</c> grid of cells, with <c>(0, 0)</c> being the top left corner, the center is 
	/// <c>(max(0, width / 2 - 1), height / 2)</c>. E.g. <c>(1, 0)</c> for a 2×1 painting, or <c>(1, 2)</c> for a 4×4 painting. 
	/// Source: wiki.vg
	/// </remarks>
	public Position CenterCoordinates { get; set; }

	/// <summary>
	/// Entity UUID of this painting.
	/// </summary>
	public Guid Uuid { get; set; }

	/// <summary>
	/// Facing direction of this painting. Matches <see cref="PaintingDirection"/>.
	/// </summary>
	public Byte Direction { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityID = stream.ReadVarInt();
		this.Uuid = stream.ReadGuid();
		this.Motive = stream.ReadVarInt();
		this.CenterCoordinates = stream.ReadPosition();
		this.Direction = stream.ReadUnsignedByte();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityID.Length + this.Motive.Length + 25);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityID);
		stream.WriteGuid(this.Uuid);
		stream.WriteVarInt(this.Motive);
		stream.WritePosition(this.CenterCoordinates);
		stream.WriteByte(this.Direction);
	}
}

