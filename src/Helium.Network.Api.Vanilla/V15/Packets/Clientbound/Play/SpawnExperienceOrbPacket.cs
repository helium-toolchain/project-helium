namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x01: Clientbound spawn experience orb packet.
/// Spawns exactly one experience orb.
/// </summary>

// this is listed on wiki.vg as spawning one or more orbs, see https://wiki.vg/index.php?title=Protocol&oldid=15871#Spawn_Experience_Orb,
// but that does not appear to be the case considering its lack of a count property. In fact, any overrolling mechanic is nullified by this
// packet sending the entity ID for... some reason. It can be assumed this specific wiki.vg revision is mistaken.
public struct SpawnExperienceOrbPacket : IPacket
{
	public VarInt Id => 0x01;

	/// <summary>
	/// Server-side entity ID of this experience orb.
	/// </summary>
	public VarInt EntityID { get; set; }

	/// <summary>
	/// Stores the amount of experience this orb rewards upon collection.
	/// </summary>
	public Int16 Value { get; set; }
	
	/// <summary>
	/// Current position of this experience orb.
	/// </summary>
	public Double PositionX { get; set; }

	/// <summary>
	/// Current position of this experience orb.
	/// </summary>
	public Double PositionY { get; set; }

	/// <summary>
	/// Current position of this experience orb.
	/// </summary>
	public Double PositionZ { get; set; }

	public void Read(PacketStream stream)
	{
		this.EntityID = stream.ReadVarInt();
		this.PositionX = stream.ReadDouble();
		this.PositionY = stream.ReadDouble();
		this.PositionZ = stream.ReadDouble();
		this.Value = stream.ReadInt16();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityID.Length + 26);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityID);
		stream.WriteDouble(this.PositionX);
		stream.WriteDouble(this.PositionY);
		stream.WriteDouble(this.PositionZ);
		stream.WriteInt16(this.Value);
	}
}
