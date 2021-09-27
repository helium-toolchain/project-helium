namespace Helium.Network.Api.Vanilla;

using Helium.Api.Mojang;

/// <summary>
/// Base interface for all packets. Specifies basic IO methods.
/// </summary>
public interface IPacket
{
	/// <summary>
	/// Packet ID of this packet.
	/// </summary>
	public VarInt Id { get; }

	/// <summary>
	/// Write the contents of this packet to the specified <see cref="PacketStream"/>
	/// <para>It is important to note the Identifier and Length parameters are not yet written by the time this is called.</para>
	/// </summary>
	public void Write(PacketStream stream);

	/// <summary>
	/// Initialize this packet from the current <see cref="PacketStream"/>
	/// <para>It is important to note the Identifier and Length parameters are already read by the time this is called.</para>
	/// </summary>
	public void Read(PacketStream stream);
}
