namespace Helium.Api.Primitives.Chat;

using System;

/// <summary>
/// Extended IChatColour that needs to be handled client-side, allowing some minor performance improvements on both sides.
/// </summary>
public interface IChatFormatting : IChatColour
{
	/// <summary>
	/// False for colours, true for all other chat formattings.
	/// </summary>
	public Boolean IsMarkup { get; }
}
