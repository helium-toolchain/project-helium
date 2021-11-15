namespace Helium.Network.Api.Vanilla.V15.Packets.Clientbound.Login;

using System;
using System.Runtime.CompilerServices;

using Helium.Api.Mojang;

/// <summary>
/// Login-C0x01: Clientbound encryption request packet.
/// </summary>
public struct EncryptionRequestPacket : IPacket
{
	public VarInt Id => 0x01;

	/// <summary>
	/// Length of the sent public key
	/// </summary>
	public VarInt PublicKeyLength { get; set; }

	/// <summary>
	/// Length of the sent verification token. Always 4 for notchian connections.
	/// </summary>
	public VarInt VerifyTokenLength { get; set; }

	/// <summary>
	/// Public encryption key
	/// </summary>
	public Byte[] PublicKey { get; set; }

	/// <summary>
	/// Verification token
	/// </summary>
	public Byte[] VerifyToken { get; set; }

	/// <summary>
	/// Server ID. Always empty with the notchian protocol (the data field is 0x00, a VarInt signaling a string length of 0)
	/// </summary>
	public String ServerId { get; set; } = null!;

	/// <summary>
	/// Calculates the length of this packet. We're just hoping this gets inlined...
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public VarInt CalculatePacketLength()
	{
		return Id.Length + PublicKeyLength.Length + PublicKeyLength + VerifyTokenLength.Length + VerifyTokenLength + 1;
	}

	public void Read(PacketStream stream)
	{
		ServerId = stream.ReadString(20);
		PublicKeyLength = stream.ReadVarInt();
		PublicKey = stream.ReadByteArray(PublicKeyLength);
		VerifyTokenLength = stream.ReadVarInt();
		VerifyToken = stream.ReadByteArray(VerifyTokenLength);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(CalculatePacketLength());
		stream.WriteVarInt(Id);
		stream.WriteString(ServerId);
		stream.WriteVarInt(PublicKeyLength);
		stream.WriteByteArray(PublicKey);
		stream.WriteVarInt(VerifyTokenLength);
		stream.WriteByteArray(VerifyToken);
	}
}
