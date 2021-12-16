namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;
using System.IO;
using System.Runtime.CompilerServices;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Meta;

/// <summary>
/// C0x0D Bossbar Packet. Creates, destroys and updates boss bars.
/// </summary>
public struct BossbarPacket : IPacket
{
	public VarInt Id => 0x0D;

	/// <summary>
	/// ID of this bossbar.
	/// </summary>
	public Guid Uuid { get; set; }

	/// <summary>
	/// Health percentage for this bossbar, 0 to 1
	/// </summary>
	/// <remarks>
	/// Values greater than 1 should not crash a client.
	/// </remarks>
	public Single Health { get; set; }

	/// <summary>
	/// Action represented by this packet. Determines the layout of the remaining packet.
	/// </summary>
	public BossbarPacketActions Action { get; set; }

	/// <summary>
	/// Bit flags. 0x01 -> client should darken the sky; 0x02 -> this is a dragon fight; 0x04 -> client should create boss fog.
	/// </summary>
	public Byte Flags { get; set; }

	/// <summary>
	/// The colour the client should use.
	/// </summary>
	public VarInt Color { get; set; }

	/// <summary>
	/// Controls the amount of notches the client should render.
	/// </summary>
	public VarInt Division { get; set; }

	/// <summary>
	/// Bossbar title, using the chat format.
	/// </summary>
	public String Title { get; set; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Int32 GetLength()
	{
		return this.Action switch
		{
			BossbarPacketActions.Add => 51 + this.Color.Length + this.Division.Length + ((VarInt)this.Title.Length).Length + this.Title.Length,
			BossbarPacketActions.Remove => 18,
			BossbarPacketActions.UpdateHealth => 50,
			BossbarPacketActions.UpdateTitle => 18 + ((VarInt)this.Title.Length).Length + this.Title.Length,
			BossbarPacketActions.UpdateStyle => 20,
			BossbarPacketActions.UpdateFlags => 19,
			_ => throw new ArgumentException($"Invalid bossbar packet action encountered: {this.Action}")
		};
	}

	public void Read(PacketStream stream)
	{
		this.Uuid = stream.ReadGuid();
		this.Action = (BossbarPacketActions)(Int32)stream.ReadVarInt();

		switch(this.Action)
		{
			case BossbarPacketActions.Add:
				this.Title = stream.ReadString(262144);
				this.Health = stream.ReadSingle();
				this.Color = stream.ReadVarInt();
				this.Division = stream.ReadVarInt();
				this.Flags = stream.ReadUnsignedByte();
				break;
			case BossbarPacketActions.Remove:
				break;
			case BossbarPacketActions.UpdateHealth:
				this.Health = stream.ReadSingle();
				break;
			case BossbarPacketActions.UpdateTitle:
				this.Title = stream.ReadString(262144);
				break;
			case BossbarPacketActions.UpdateStyle:
				this.Color = stream.ReadVarInt();
				this.Division = stream.ReadVarInt();
				break;
			case BossbarPacketActions.UpdateFlags:
				this.Flags = stream.ReadUnsignedByte();
				break;
			default:
				throw new InvalidDataException($"Invalid bossbar packet action encountered: {this.Action}");
		}
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.GetLength());
		stream.WriteVarInt(this.Id);
		stream.WriteGuid(this.Uuid);
		stream.WriteVarInt((Int32)this.Action);

		switch(this.Action)
		{
			case BossbarPacketActions.Add:
				stream.WriteString(this.Title);
				stream.WriteSingle(this.Health);
				stream.WriteVarInt(this.Color);
				stream.WriteVarInt(this.Division);
				stream.WriteUnsignedByte(this.Flags);
				break;
			case BossbarPacketActions.Remove:
				break;
			case BossbarPacketActions.UpdateHealth:
				stream.WriteSingle(this.Health);
				break;
			case BossbarPacketActions.UpdateTitle:
				stream.WriteString(this.Title);
				break;
			case BossbarPacketActions.UpdateStyle:
				stream.WriteVarInt(this.Color);
				stream.WriteVarInt(this.Division);
				break;
			case BossbarPacketActions.UpdateFlags:
				stream.WriteUnsignedByte(this.Flags);
				break;
			default:
				throw new ArgumentException($"Invalid bossbar packet action encountered: {this.Action}");
		}
	}
}
