namespace Helium.Network.Api.Vanilla.Packets;

/// <summary>
/// Base interface for all legacy packets. Specifies basic IO methods.
/// </summary>
public interface ILegacyPacket
{
	/// <summary>
	/// Write the contents of this packet to the specified <see cref="PacketStream"/>
	/// <para>It is important to note the Identifier parameter is not yet written by the time this is called.</para>
	/// </summary>
	public void Write(PacketStream stream);

	/// <summary>
	/// Initialize this packet from the current <see cref="PacketStream"/>
	/// <para>It is important to note the Identifier parameter ia already read by the time this is called.</para>
	/// </summary>
	public void Read(PacketStream stream);
}
