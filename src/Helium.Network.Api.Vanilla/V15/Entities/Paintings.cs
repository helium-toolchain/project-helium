namespace Helium.Network.Api.Vanilla.V15.Entities;

using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

/// <summary>
/// A full list of all vanilla-supported entities. The enum index should match <see cref="SpawnPaintingPacket.Motive"/>, 
/// the minecraft-namespaced name is obtained by converting the enum name to snake case and prepending it with <c>minecraft:</c>.
/// </summary>
/// <remarks>
///	Exception: <see cref="SkullAndRoses"/> does not use the <c>minecraft:</c> namespace prefix, instead it is unnamespaced.
///	</remarks>
public enum Paintings
{
	Kebab,
	Aztec,
	Alban,
	Aztec2,
	Bomb,
	Plant,
	Wasteland,
	Pool,
	Courbet,
	Sea,
	Sunset,
	Creebet,
	Wanderer,
	Graham,
	Match,
	Bust,
	Stage,
	Void,
	SkullAndRoses,
	Wither,
	Fighters,
	Pointer,
	Pigscene,
	BurningSkull,
	Skeleton,
	DonkeyKong
}

