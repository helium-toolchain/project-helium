namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using Helium.Api.Mojang;

/// <summary>
/// C0x0C - Block Change Packet
/// </summary>
public struct BlockChangePacket : IPacket
{
	public VarInt Id { get; }

	/// <summary>
	/// Describes the changed block location.
	/// </summary>
	public Position Location { get; set; }

	/// <summary>
	/// The new block state ID at this location.
	/// </summary>
	public VarInt BlockStateId { get; set; }

	public void Read(PacketStream stream)
	{
		this.Location = stream.ReadPosition();
		this.BlockStateId = stream.ReadVarInt();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.BlockStateId.Length + 8);
		stream.WriteVarInt(this.Id);
		stream.WritePosition(this.Location);
		stream.WriteVarInt(this.BlockStateId);
	}
}
