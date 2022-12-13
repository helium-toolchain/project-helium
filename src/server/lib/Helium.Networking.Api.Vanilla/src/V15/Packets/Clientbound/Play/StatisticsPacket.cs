namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

using System;

using Helium.Api.Mojang;

/// <summary>
/// C0x07: Statistics Packet. Only sent as response to S0x04.
/// </summary>

// technically, Count is pointless since the client doesn't cache them anyway. Even to keep mod compatibility... the mods could just tell you
// what new count you have to expect? Mojang...
public struct StatisticsPacket : IPacket
{
	public VarInt Id => 0x07;

	/// <summary>
	/// The amount of entries in the following array.
	/// </summary>
	public VarInt Count { get; set; }

	/// <summary>
	/// An array of all sent statistics. The client does not cache statistics.
	/// </summary>
	public Statistic[] Statistics { get; set; }

	public void Read(PacketStream stream)
	{
		this.Count = stream.ReadVarInt();
		this.Statistics = new Statistic[Count];
		Statistic intermediary = new();

		for(Int32 i = 0; i < Count; i++)
		{
			intermediary.CategoryId = stream.ReadVarInt();
			intermediary.StatisticId = stream.ReadVarInt();
			intermediary.Value = stream.ReadVarInt();

			Statistics[i] = intermediary;
		}
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(this.Id.Length + this.Count.Length + Statistics.CalculateLength());
		stream.WriteVarInt(this.Id);
		stream.WriteVarInt(this.Count);
		
		foreach(Statistic s in this.Statistics)
		{
			stream.WriteVarInt(s.CategoryId);
			stream.WriteVarInt(s.StatisticId);
			stream.WriteVarInt(s.Value);
		}
	}
}

