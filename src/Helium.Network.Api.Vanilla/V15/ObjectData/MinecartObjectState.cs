namespace Helium.Network.Api.Vanilla.V15.ObjectData;

/// <summary>
/// Mirrors the different object states of minecarts as sent via C0x00.Data.
/// </summary>
public enum MinecartObjectState
{
	/// <summary>
	/// An empty - rideable - minecart.
	/// </summary>
	Empty,

	/// <summary>
	/// A chest minecart (because using a different ID would be too hard).
	/// </summary>
	Chest,

	/// <summary>
	/// A furnace minecart.
	/// </summary>
	Furnace,

	/// <summary>
	/// A TNT minecart.
	/// </summary>
	Tnt,

	/// <summary>
	/// A spawner minecart.
	/// </summary>
	Spawner,

	/// <summary>
	/// A hopper minecart.
	/// </summary>
	Hopper,

	/// <summary>
	/// A command block minecart.
	/// </summary>
	CommandBlock
}
