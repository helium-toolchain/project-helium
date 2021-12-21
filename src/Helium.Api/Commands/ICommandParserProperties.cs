namespace Helium.Api.Commands;

using System.IO;

/// <summary>
/// Base interface for specific parsers for command arguments.
/// </summary>
public interface ICommandParserProperties
{
	/// <summary>
	/// Reads property data from a stream - this must be a PacketStream for the Helium implementation
	/// (found in Helium.Network.Api.Vanilla); but other implementations can use different streams.
	/// </summary>
	public void Read(Stream stream);

	/// <summary>
	/// Writes property data to a stream - this must be a PacketStream for the Helium implementation
	/// (found in Helium.Network.Api.Vanilla); but other implementations can use different streams.
	/// </summary>
	public void Write(Stream stream);
}
