namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x05: Spawn player packet. Must be sent AFTER the corresponding <see cref="PlayerInfoPacket"/> is sent if notchian clients are
/// meant to handle the player in question.
/// </summary>
public struct SpawnPlayerPacket : IPacket
{
	public VarInt Id => 0x05;

	/// <summary>
	/// Server-side entity ID of this player.
	/// </summary>
	public VarInt EntityID { get; set; }

	/// <summary>
	/// Player UUID. The UUIDs must be valid and have valid skin blobs on mojangs servers.
	/// </summary>
	/// <remarks>
	/// For NPCs, UUID v2 should be used.
	/// </remarks>
	public Guid PlayerUuid { get; set; }

	/// <summary>
	/// Coordinates of this player.
	/// </summary>
	public Double PositionX { get; set; }

	/// <summary>
	/// Coordinates of this player.
	/// </summary>
	public Double PositionY { get; set; }

	/// <summary>
	/// Coordinates of this player.
	/// </summary>
	public Double PositionZ { get; set; }

	/// <summary>
	/// Camera/Line-of-sight rotation of this player.
	/// </summary>
	public Single Yaw { get; set; }

	/// <summary>
	/// Camera/Line-of-sight rotation of this player.
	/// </summary>
	public Single Pitch { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityID = stream.ReadVarInt();
		this.PlayerUuid = stream.ReadGuid();
		this.PositionX = stream.ReadDouble();
		this.PositionY = stream.ReadDouble();
		this.PositionZ = stream.ReadDouble();
		this.Yaw = stream.ReadAngle();
		this.Pitch = stream.ReadAngle();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityID.Length + 48);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityID);
		stream.WriteGuid(this.PlayerUuid);
		stream.WriteDouble(this.PositionX);
		stream.WriteDouble(this.PositionY);
		stream.WriteDouble(this.PositionZ);
		stream.WriteAngle(this.Yaw);
		stream.WriteAngle(this.Pitch);
	}
}

