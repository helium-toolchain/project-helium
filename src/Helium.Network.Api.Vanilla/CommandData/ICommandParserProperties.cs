namespace Helium.Network.Api.Vanilla.CommandData;

public interface ICommandParserProperties
{
	public void Read(PacketStream stream);
	public void Write(PacketStream stream);
}
