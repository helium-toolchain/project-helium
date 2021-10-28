namespace Helium.Network.Api.Vanilla.V15;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

/// <summary>
/// Holds statistics data sent by <see cref="StatisticsPacket"/>
/// </summary>
public record struct Statistic
{
	/// <summary>
	/// Category ID of this statistic, see <see cref="StatisticCategories"/>
	/// </summary>
	public VarInt CategoryId { get; set; }

	/// <summary>
	/// Statistic ID inside the statistic, for <see cref="StatisticCategories.Custom"/> these are listed in <see cref="CustomStatistics"/>
	/// </summary>
	public VarInt StatisticId { get; set; }

	/// <summary>
	/// Integer value.
	/// </summary>
	public VarInt Value { get; set; }

	/// <summary>
	/// Protocol length of this statistic.
	/// </summary>
	public Int32 Length => CategoryId.Length + StatisticId.Length + Value.Length;
}

