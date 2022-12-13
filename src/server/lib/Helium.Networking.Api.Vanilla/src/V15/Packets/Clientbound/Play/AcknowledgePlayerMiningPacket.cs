namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x08: Acknowledges the player mining a block
/// </summary>
public struct AcknowledgePlayerMiningPacket : IPacket
{
	public VarInt Id => 0x08;

	/// <summary>
	/// Block position
	/// </summary>
	public Position Location { get; set; }

	/// <summary>
	/// Registry blockstate ID of the block in question
	/// </summary>
	public VarInt Blockstate { get; set; }

	/// <summary>
	/// 0 - started digging, 1 - cancelled digging, 2 - finished digging.
	/// </summary>
	public VarInt Status { get; set; }

	/// <summary>
	/// True if the server accepts the action.
	/// </summary>
	public Boolean Success { get; set; }

	public void Read(PacketStream stream)
	{
		this.Location = stream.ReadPosition();
		this.Blockstate = stream.ReadVarInt();
		this.Status = stream.ReadVarInt();
		this.Success = stream.ReadBoolean();
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.Blockstate.Length + this.Status.Length + 9);
		stream.WriteVarInt(this.Id);
		stream.WritePosition(this.Location);
		stream.WriteVarInt(this.Blockstate);
		stream.WriteVarInt(this.Status);
		stream.WriteBoolean(this.Success);
	}
}
