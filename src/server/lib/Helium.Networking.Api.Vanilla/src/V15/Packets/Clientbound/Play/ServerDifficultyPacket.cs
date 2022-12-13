namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x0E: Set Server Difficulty
/// </summary>
public struct ServerDifficultyPacket : IPacket
{
	public VarInt Id => 0x0E;

	/// <summary>
	/// Indicates the new server difficulty.
	/// </summary>
	public Byte Difficulty { get; set; }

	/// <summary>
	/// Indicates whether the difficulty is locked.
	/// </summary>
	public Boolean Locked { get; set; }

	public void Read(PacketStream stream)
	{
		this.Difficulty = stream.ReadUnsignedByte();
		this.Locked = stream.ReadBoolean();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(3);
		stream.WriteVarInt(this.Id);
		stream.WriteUnsignedByte(this.Difficulty);
		stream.WriteBoolean(this.Locked);
	}
}
