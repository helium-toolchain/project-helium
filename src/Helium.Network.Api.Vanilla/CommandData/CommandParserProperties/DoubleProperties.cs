namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>brigadier:double</c> command arguments.
/// </summary>
public sealed class DoubleProperties : ICommandParserProperties
{
	/// <summary>
	/// Represents the minimum allowed value for this double.
	/// </summary>
	public Double MinValue { get; private set; } = Double.MinValue;

	/// <summary>
	/// Represents the maximum allowed value for this double.
	/// </summary>
	public Double MaxValue { get; private set; } = Double.MaxValue;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		Byte flags = stream.ReadUnsignedByte();

		if((flags & 0x01) != 0)
		{
			this.MinValue = stream.ReadDouble();
		}

		if((flags & 0x02) != 0)
		{
			this.MaxValue = stream.ReadDouble();
		}
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		if(this.MinValue == Double.MinValue && this.MaxValue == Double.MaxValue)
		{
			stream.WriteUnsignedByte(0x00);
			return;
		}

		if(this.MaxValue == Double.MaxValue)
		{
			stream.WriteUnsignedByte(0x01);
			stream.WriteDouble(this.MinValue);
			return;
		}

		if(this.MinValue == Double.MinValue)
		{
			stream.WriteUnsignedByte(0x02);
			stream.WriteDouble(this.MaxValue);
			return;
		}

		stream.WriteUnsignedByte(0x03);
		stream.WriteDouble(this.MinValue);
		stream.WriteDouble(this.MaxValue);
	}
}
