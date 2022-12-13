namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x02 - Clientbound Spawn Weather Entity Packet.
/// This packet notifies the client of a thunderbolt strike within 512 blocks of the player.
/// </summary>

// why 512 blocks? there's absolutely no reason to notify the client of a thunderbolt strike in an area that isnt even sent to this client
// also why does this contain the Y coordinate? lightning bolts always strike the highest block in their xz column
public struct SpawnWeatherEntityPacket : IPacket
{
	public VarInt Id => 0x02;

	/// <summary>
	/// Serverside thunderbolt entity ID.
	/// </summary>
	public VarInt EntityID { get; set; }

	/// <summary>
	/// Target coordinates of this thunderbolt strike.
	/// </summary>
	public Double TargetX { get; set; }

	/// <summary>
	/// Target coordinates of this thunderbolt strike.
	/// </summary>
	public Double TargetY { get; set; }

	/// <summary>
	/// Target coordinates of this thunderbolt strike.
	/// </summary>
	public Double TargetZ { get; set; }

	/// <summary>
	/// Global weather entity type, always 1
	/// </summary>
	public Byte Type { get; set; } // Mojang tried to support having more weather entities here. i suppose it makes sense to keep in for mods

	public void Read(PacketStream stream)
	{
		this.EntityID = stream.ReadVarInt();
		this.Type = stream.ReadUnsignedByte();
		this.TargetX = stream.ReadDouble();
		this.TargetY = stream.ReadDouble();
		this.TargetZ = stream.ReadDouble();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.EntityID.Length + 25);
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.EntityID);
		stream.WriteByte(this.Type);
		stream.WriteDouble(this.TargetX);
		stream.WriteDouble(this.TargetY);
		stream.WriteDouble(this.TargetZ);
	}
}
