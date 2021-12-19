namespace Helium.Network.Api.Vanilla.CommandData;

using System;

using Helium.Api.Mojang;

/// <summary>
/// Represents one command node.
/// </summary>
/// <remarks>
/// The <see cref="VarInt"/>s stored here refer to indices on the node stack. This record is completely useless by itself,
/// without a data converter tool to bring this into a remotely reasonable format.
/// </remarks>
public record struct CommandNode
{
#nullable enable
	/// <summary>
	/// Flags associated with this node.
	/// </summary>
	public CommandNodeFlags Flags { get; set; }

	/// <summary>
	/// The length of the following array.
	/// </summary>
	public VarInt ChildrenCount { get; set; }

	/// <summary>
	/// Indices of all child nodes in the node stack.
	/// </summary>
	public VarInt[] Children { get; set; }

	/// <summary>
	/// Only present if <see cref="Flags"/> == <see cref="CommandNodeFlags.Redirects"/>. 
	/// Index of the node this redirect points to.
	/// </summary>
	public VarInt? RedirectNode { get; set; }

	/// <summary>
	/// Name of this node. Only present if <see cref="Flags"/> == <see cref="CommandNodeFlags.NodeTypeMask"/> exists in one 
	/// fashion or another.
	/// </summary>
	public String? Name { get; set; }

	/// <summary>
	/// The <see cref="CommandParserTypes"/> used to parse this node.
	/// Only present if <see cref="Flags"/> == 0x02.
	/// </summary>
	public String? Parser { get; set; }

	/// <summary>
	/// Additional parser properties for this node. Only present if <see cref="Flags"/> == 0x02
	/// </summary>
	public ICommandParserProperties? Properties { get; set; }

	/// <summary>
	/// Datatype used for autocomplete suggestion handling, see <see cref="CommandSuggestionBehaviourTypes"/>.
	/// Only present if <see cref="Flags"/> == <see cref="CommandNodeFlags.Suggestions"/>
	/// </summary>
	public String? SuggestionType { get; set; }
#nullable restore
}
