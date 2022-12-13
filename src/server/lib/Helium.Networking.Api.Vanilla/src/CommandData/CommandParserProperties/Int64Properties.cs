namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>brigadier:long</c> command arguments.
/// </summary>
internal class Int64Properties : ICommandParserProperties
{
	/// <summary>
	/// Represents the minimum allowed value for this int64.
	/// </summary>
	public Int64 MinValue { get; private set; } = Int64.MinValue;

	/// <summary>
	/// Represents the maximum allowed value for this int64.
	/// </summary>
	public Int64 MaxValue { get; private set; } = Int64.MaxValue;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		Byte flags = stream.ReadUnsignedByte();

		if((flags & 0x01) != 0)
		{
			this.MinValue = stream.ReadInt64();
		}

		if((flags & 0x02) != 0)
		{
			this.MaxValue = stream.ReadInt64();
		}
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		if(this.MinValue == Int64.MinValue && this.MaxValue == Int64.MaxValue)
		{
			stream.WriteUnsignedByte(0x00);
			return;
		}

		if(this.MaxValue == Int64.MaxValue)
		{
			stream.WriteUnsignedByte(0x01);
			stream.WriteInt64(this.MinValue);
			return;
		}

		if(this.MinValue == Int64.MinValue)
		{
			stream.WriteUnsignedByte(0x02);
			stream.WriteInt64(this.MaxValue);
			return;
		}

		stream.WriteUnsignedByte(0x03);
		stream.WriteInt64(this.MinValue);
		stream.WriteInt64(this.MaxValue);
	}
}
