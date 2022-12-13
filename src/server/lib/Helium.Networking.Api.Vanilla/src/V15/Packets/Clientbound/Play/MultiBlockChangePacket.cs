namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x10: Multiple Blocks Changed
/// </summary>
public struct MultiBlockChangePacket : IPacket
{
	public VarInt Id => 0x10;

	/// <summary>
	/// X coordinate of the parent chunk.
	/// </summary>
	public Int32 ChunkXCoordinate { get; set; }

	/// <summary>
	/// Z coordinate of the parent chunk.
	/// </summary>
	public Int32 ChunkZCoordinate { get; set; }

	/// <summary>
	/// Amount of changes in the following array.
	/// </summary>
	public VarInt ChangeCount { get; set; }

	/// <summary>
	/// All block changes in the declared chunk.
	/// </summary>
	public BlockChangeData[] Changes { get; set; }

	public void Read(PacketStream stream)
	{
		this.ChunkXCoordinate = stream.ReadInt32();
		this.ChunkZCoordinate = stream.ReadInt32();
		this.ChangeCount = stream.ReadVarInt();

		this.Changes = new BlockChangeData[this.ChangeCount];

		for(Int32 i = 0; i < this.ChangeCount; i++)
		{
			this.Changes[i].HorizontalPosition = stream.ReadUnsignedByte();
			this.Changes[i].YCoordinate = stream.ReadUnsignedByte();
			this.Changes[i].Blockstate = stream.ReadVarInt();
		}
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(9 + this.ChangeCount.Length + Changes.CalculateLength());
		stream.WriteVarInt(this.Id);
		stream.WriteInt32(this.ChunkXCoordinate);
		stream.WriteInt32(this.ChunkZCoordinate);
		stream.WriteVarInt(this.ChangeCount);

		foreach(BlockChangeData b in this.Changes)
		{
			stream.WriteUnsignedByte(b.HorizontalPosition);
			stream.WriteUnsignedByte(b.YCoordinate);
			stream.WriteVarInt(b.Blockstate);
		}
	}
}
