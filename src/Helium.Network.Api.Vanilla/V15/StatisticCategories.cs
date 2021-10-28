namespace Helium.Network.Api.Vanilla.V15;

/// <summary>
/// Stores all statistic categories used by the vanilla protocol.
/// To translate those into minecraft's ID names, convert them to snake case and prepend <c>minecraft.</c>
/// </summary>
public enum StatisticCategories
{
	Mined,
	Crafted,
	Used,
	Broken,
	PickedUp,
	Dropped,
	Killed,
	KilledBy,
	Custom
}

