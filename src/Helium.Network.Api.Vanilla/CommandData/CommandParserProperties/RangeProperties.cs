namespace Helium.Network.Api.Vanilla.CommandData.CommandParserProperties;

using System;
using System.IO;

using Helium.Api.Commands;

/// <summary>
/// Serializes and deserializes argument properties for <c>minecraft:range</c> command arguments.
/// </summary>
internal class RangeProperties : ICommandParserProperties
{
	/// <summary>
	/// Controls usage of decimals within the given range.
	/// </summary>
	public Boolean AllowDecimals { get; set; } = false;

	public void Read(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		this.AllowDecimals = stream.ReadBoolean();
	}

	public void Write(Stream s)
	{
		if(s is not PacketStream stream)
		{
			throw new ArgumentException($"PacketStream expected, found {s.GetType()}");
		}

		stream.WriteBoolean(this.AllowDecimals);
	}
}
