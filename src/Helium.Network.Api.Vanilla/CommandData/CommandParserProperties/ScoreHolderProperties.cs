namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>minecraft:score_holder</c> command arguments.
/// </summary>
internal class ScoreHolderProperties : ICommandParserProperties
{
	/// <summary>
	/// 0x01 -> allow multiple
	/// </summary>
	public Byte Flags { get; set; } = 0;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		this.Flags = stream.ReadUnsignedByte();
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		stream.WriteUnsignedByte(this.Flags);
	}
}
