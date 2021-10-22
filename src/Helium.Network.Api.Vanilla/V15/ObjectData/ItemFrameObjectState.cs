namespace Helium.Network.Api.Vanilla.V15.ObjectData;

/// <summary>
/// Mirrors the different object states of item frames as sent via C0x00.Data
/// </summary>
public enum ItemFrameObjectState
{
	/// <summary>
	/// The item frame is facing downwards.
	/// </summary>
	Down,

	/// <summary>
	/// The item frame is facing upwards.
	/// </summary>
	Up,

	/// <summary>
	/// The item frame is facing north.
	/// </summary>
	North,

	/// <summary>
	/// The item frame is facing south.
	/// </summary>
	South,

	/// <summary>
	/// The item frame is facing west.
	/// </summary>
	West,

	/// <summary>
	/// The item frame is facing east.
	/// </summary>
	East
}
