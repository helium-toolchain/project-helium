namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Status;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla;

/// <summary>
/// Status-S0x00: Serverbound request initiation packet
/// </summary>
public struct RequestPacket : IPacket
{
	public VarInt Id => 0x00;

	public void Read(PacketStream stream)
	{
		return;
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length);
		stream.WriteVarInt(Id);
	}
}
