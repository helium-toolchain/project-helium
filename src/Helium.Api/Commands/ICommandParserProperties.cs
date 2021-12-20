namespace Helium.Api.Commands;

public interface ICommandParserProperties
{
	public void Read(PacketStream stream);
	public void Write(PacketStream stream);
}
