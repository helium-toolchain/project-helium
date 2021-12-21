namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>brigadier:integer</c> command arguments.
/// </summary>
internal class Int32Properties : ICommandParserProperties
{
	/// <summary>
	/// Represents the minimum allowed value for this int32.
	/// </summary>
	public Int32 MinValue { get; private set; } = Int32.MinValue;

	/// <summary>
	/// Represents the maximum allowed value for this int32.
	/// </summary>
	public Int32 MaxValue { get; private set; } = Int32.MaxValue;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		Byte flags = stream.ReadUnsignedByte();

		if((flags & 0x01) != 0)
		{
			this.MinValue = stream.ReadInt32();
		}

		if((flags & 0x02) != 0)
		{
			this.MaxValue = stream.ReadInt32();
		}
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		if(this.MinValue == Int32.MinValue && this.MaxValue == Int32.MaxValue)
		{
			stream.WriteUnsignedByte(0x00);
			return;
		}

		if(this.MaxValue == Int32.MaxValue)
		{
			stream.WriteUnsignedByte(0x01);
			stream.WriteInt32(this.MinValue);
			return;
		}

		if(this.MinValue == Int32.MinValue)
		{
			stream.WriteUnsignedByte(0x02);
			stream.WriteInt32(this.MaxValue);
			return;
		}

		stream.WriteUnsignedByte(0x03);
		stream.WriteInt32(this.MinValue);
		stream.WriteInt32(this.MaxValue);
	}
}
