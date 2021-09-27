namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Status;

using System;

using Helium.Api.Mojang;
using Helium.Network.Api.Vanilla;

/// <summary>
/// Status-C0x00: Clientbound Response packet, identifier 0x00.
/// </summary>
public class ResponsePacket : IPacket
{
	public VarInt Id => 0x00;

	/// <summary>
	/// Length of the response.
	/// </summary>
	public VarInt Length { get; set; }

	/// <summary>
	/// The JSON response with a maximum length of 32767
	/// </summary>
	public String Response { get; set; }

	public void Read(PacketStream stream)
	{
		this.Length = stream.ReadVarInt();
		this.Response = stream.ReadRawString(Length);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + Length.Length + Response.Length);
		stream.WriteVarInt(0x00);
		stream.WriteVarInt(Length);
		stream.WriteRawString(Response);
	}
}
