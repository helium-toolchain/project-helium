namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Meta;

using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

/// <summary>
/// Represents the actions that can be performed in <see cref="BossbarPacket"/>.
/// </summary>
public enum BossbarPacketActions
{
	/// <summary>
	/// Create a new bossbar on the client.
	/// </summary>
	Add,

	/// <summary>
	/// Remove a bossbar from the client.
	/// </summary>
	Remove,

	/// <summary>
	/// Update health represented by a bossbar.
	/// </summary>
	UpdateHealth,

	/// <summary>
	/// Update the title of a bossbar.
	/// </summary>
	UpdateTitle,

	/// <summary>
	/// Update the visual design of a bossbar.
	/// </summary>
	UpdateStyle,

	/// <summary>
	/// Update the flags associated with a bossbar.
	/// </summary>
	UpdateFlags
}
