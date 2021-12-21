namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>brigadier:float</c> command arguments.
/// </summary>
internal class SingleProperties : ICommandParserProperties
{
	/// <summary>
	/// Represents the minimum allowed value for this float.
	/// </summary>
	public Single MinValue { get; private set; } = Single.MinValue;

	/// <summary>
	/// Represents the maximum allowed value for this float.
	/// </summary>
	public Single MaxValue { get; private set; } = Single.MaxValue;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		Byte flags = stream.ReadUnsignedByte();

		if((flags & 0x01) != 0)
		{
			this.MinValue = stream.ReadSingle();
		}

		if((flags & 0x02) != 0)
		{
			this.MaxValue = stream.ReadSingle();
		}
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		if(this.MinValue == Single.MinValue && this.MaxValue == Single.MaxValue)
		{
			stream.WriteUnsignedByte(0x00);
			return;
		}

		if(this.MaxValue == Single.MaxValue)
		{
			stream.WriteUnsignedByte(0x01);
			stream.WriteSingle(this.MinValue);
			return;
		}

		if(this.MinValue == Single.MinValue)
		{
			stream.WriteUnsignedByte(0x02);
			stream.WriteSingle(this.MaxValue);
			return;
		}

		stream.WriteUnsignedByte(0x03);
		stream.WriteSingle(this.MinValue);
		stream.WriteSingle(this.MaxValue);
	}
}
