namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Entities;

/// <summary>
/// C0x0A: Set block entity data.
/// </summary>
public struct BlockEntityDataPacket : IPacket
{
	public VarInt Id => 0x0A;

	/// <summary>
	/// Describes the position of the block entity in question.
	/// </summary>
	public Position Location { get; set; }

	/// <summary>
	/// The operation type as defined by <see cref="BlockEntityActionTypes"/>.
	/// </summary>
	public Byte Action { get; set; }

	/// <summary>
	/// The NBT data to apply to this block entity.
	/// </summary>
	public Byte[] NbtData { get; set; }

	private Int32 PacketLength { get; set; }

	public void Read(PacketStream stream)
	{
		this.Location = stream.ReadPosition();
		this.Action = stream.ReadUnsignedByte();
		this.NbtData = stream.ReadByteArray(this.PacketLength - (this.Id.Length + 9));
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + 9 + this.NbtData.Length);
		stream.WriteVarInt(this.Id.Length);
		stream.WritePosition(this.Location);
		stream.WriteUnsignedByte(this.Action);
		stream.WriteByteArray(this.NbtData);
	}

	public BlockEntityDataPacket(Int32 packetLength)
	{
		this.PacketLength = packetLength;
		this.Location = default;
		this.Action = default;
		this.NbtData = default;
	}
}
