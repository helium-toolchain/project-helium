namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x00: Clientbound spawn entity packet.
/// Sent by the server when any non-living entity is created.
/// </summary>
public struct SpawnEntityPacket : IPacket
{
	public VarInt Id => 0x00;
	
	/// <summary>
	/// Server-side entity ID of this entity.
	/// </summary>
	// why does minecraft send this, the client has no reason to know the server-internal EID
	public VarInt EntityId { get; set; }

	/// <summary>
	/// The entity type, same as in <see cref="SpawnLivingEntityPacket"/>, relevant for <see cref="Data"/>.
	/// </summary>
	public VarInt Type { get; set; }

	/// <summary>
	/// Additional entity data. See <see cref="ObjectDataExtensions"/> and <see cref="Type"/>.
	/// </summary>
	public Int32 Data { get; set; }

	/// <summary>
	/// Rotation of the entitys line-of-sight. Serialized to a <see cref="Byte"/>, precision is not preserved fully.
	/// </summary>
	public Single Pitch { get; set; }

	/// <summary>
	/// Rotation of the entitys line-of-sight. Serialized to a <see cref="Byte"/>, precision is not preserved fully.
	/// </summary>
	public Single Yaw { get; set; }

	/// <summary>
	/// Entity velocity. Always sent, but only used if <see cref="Data"/> > 0 (save some entities where different rules apply).
	/// </summary>
	public Int16 VelocityX { get; set; }

	/// <summary>
	/// Entity velocity. Always sent, but only used if <see cref="Data"/> > 0 (save some entities where different rules apply).
	/// </summary>
	public Int16 VelocityY { get; set; }

	/// <summary>
	/// Entity velocity. Always sent, but only used if <see cref="Data"/> > 0 (save some entities where different rules apply).
	/// </summary>
	public Int16 VelocityZ { get; set; }

	/// <summary>
	/// Entity position.
	/// </summary>
	public Double PositionX { get; set; }

	/// <summary>
	/// Entity position.
	/// </summary>
	public Double PositionY { get; set; }

	/// <summary>
	/// Entity position.
	/// </summary>
	public Double PositionZ { get; set; }

	/// <summary>
	/// Server-side entity UUID
	/// </summary>
	public Guid Uuid { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityId = stream.ReadVarInt();
		this.Uuid = stream.ReadGuid();
		this.Type = stream.ReadVarInt();
		this.PositionX = stream.ReadDouble();
		this.PositionY = stream.ReadDouble();
		this.PositionZ = stream.ReadDouble();
		this.Pitch = stream.ReadAngle();
		this.Yaw = stream.ReadAngle();
		this.Data = stream.ReadInt32();
		this.VelocityX = stream.ReadInt16();
		this.VelocityY = stream.ReadInt16();
		this.VelocityZ = stream.ReadInt16();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityId.Length + this.Type.Length + 58);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityId);
		stream.WriteGuid(this.Uuid);
		stream.WriteVarInt(this.Type);
		stream.WriteDouble(this.PositionX);
		stream.WriteDouble(this.PositionY);
		stream.WriteDouble(this.PositionZ);
		stream.WriteAngle(this.Pitch);
		stream.WriteAngle(this.Yaw);
		stream.WriteInt32(this.Data);
		stream.WriteInt16(this.VelocityX);
		stream.WriteInt16(this.VelocityY);
		stream.WriteInt16(this.VelocityZ);
	}
}
