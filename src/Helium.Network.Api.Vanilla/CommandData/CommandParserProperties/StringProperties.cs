namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;
using Helium.Api.Mojang;

/// <summary>
/// Serializes and deserializes argument properties for <c>brigadier:string</c> command arguments.
/// </summary>
internal class StringProperties : ICommandParserProperties
{
	/// <summary>
	/// 0 -> single word; 1 -> quote-enclosed string; 2 -> entire string until the end
	/// </summary>
	public VarInt Behaviour { get; set; } = 2;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		this.Behaviour = stream.ReadVarInt();
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		stream.WriteVarInt(this.Behaviour);
	}
}
