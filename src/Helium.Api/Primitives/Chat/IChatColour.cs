namespace Helium.Api.Primitives.Chat;

/// <summary>
/// Base interface for chat colours.
/// </summary>
public interface IChatColour
{
	/// <summary>
	/// Colour hex code, stored as Int32. Can be omitted if this is constructed via IChatFormatting.
	/// </summary>
	public Int32? Colour { get; }

	/// <summary>
	/// Colour code ID. This should be automatically assigned by a registry and is only used for the Helium network protocol.
	/// </summary>
	public Int16 Id { get; }

	/// <summary>
	/// Optional format code, for use in paragraph sign sequences ("§atext")
	/// </summary>
	public Char? FormatCode { get; }

	/// <summary>
	/// Json format name of the colour code. If this is no vanilla format code, this should be namespaced.
	/// </summary>
	public String JsonFormatText { get; }
}
