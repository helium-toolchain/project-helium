namespace Helium.Network.Api.Vanilla.CommandData;

using System;

/// <summary>
/// Represents the allowed flags for a <see cref="CommandNode"/>.
/// </summary>
/// <remarks>
/// This does not act as a conventional flag enum. Instead, the first flag is 0x03 and acts as a bit mask; 
/// its output needs to be processed further: 0x00 -> root node; 0x01 -> literal node; 0x02 -> argument node.
/// 0x03 is invalid and should never appear.
/// </remarks>
[Flags]
public enum CommandNodeFlags : Byte
{
	/// <summary>
	/// This is in part present for all instances, see the remarks on the parent enum documentation.
	/// </summary>
	NodeTypeMask = 0x03,

	/// <summary>
	/// Specifies whether the node stack up to this point can be executed.
	/// </summary>
	Executable = 0x04,

	/// <summary>
	/// Specifies whether this node aliases another node.
	/// </summary>
	Redirects = 0x08,

	/// <summary>
	/// Specifies whether <see cref="CommandNode.SuggestionType"/> is set.
	/// </summary>
	Suggestions = 0x10
}
