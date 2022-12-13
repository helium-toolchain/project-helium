namespace Helium.Network.Api.Vanilla.V15.Packets.Serverbound.Login;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Login-S0x01: Serverbound encryption response packet. Wholly unused by Helium.
/// </summary>
public struct EncryptionResponsePacket : IPacket
{
	public VarInt Id => 0x01;

	/// <summary>
	/// Length of the shared secret.
	/// </summary>
	public VarInt SharedSecretLength { get; set; }

	/// <summary>
	/// Length of the verify token.
	/// </summary>
	public VarInt VerifyTokenLength { get; set; }

	/// <summary>
	/// Shared secret.
	/// </summary>
	public Byte[] SharedSecret { get; set; }

	/// <summary>
	/// Verify token, always 4 bytes long in a notchian connection.
	/// </summary>
	public Byte[] VerifyToken { get; set; }

	public void Read(PacketStream stream)
	{
		SharedSecretLength = stream.ReadVarInt();
		SharedSecret = stream.ReadByteArray(SharedSecretLength);
		VerifyTokenLength = stream.ReadVarInt();
		VerifyToken = stream.ReadByteArray(VerifyTokenLength);
	}

	public void Write(PacketStream stream)
	{
		stream.WriteVarInt(Id.Length + SharedSecretLength.Length + SharedSecretLength +
			VerifyTokenLength.Length + VerifyTokenLength);
		stream.WriteVarInt(Id);
		stream.WriteVarInt(SharedSecretLength);
		stream.WriteByteArray(SharedSecret);
		stream.WriteVarInt(VerifyTokenLength);
		stream.WriteByteArray(VerifyToken);
	}
}
