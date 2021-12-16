namespace Helium.Network.Api.Vanilla.V15.Entities;

using Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Play;

/// <summary>
/// A full list of all valid entity data updates in <see cref="BlockEntityDataPacket"/>.
/// </summary>
public enum BlockEntityActionTypes
{
	SetMobSpawnerData,
	SetCommandBlockText,
	SetBeaconData,
	SetHeadData,
	DeclareConduit,
	SetBannerData,
	SetStructureBlockEntityData,
	SetEndGatewayDestination,
	SetSignText,
	DeclareBed = 11,
	SetJigsawData,
	SetCampfireItems,
	SetBeehiveData
}
