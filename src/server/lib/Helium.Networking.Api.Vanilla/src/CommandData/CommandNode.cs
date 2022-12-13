namespace Helium.Network.Api.Vanilla.CommandData;

using System;
using System.Collections.Generic;
using System.Linq;

using Helium.Api.Commands;
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

	public static CommandNode FromHeliumNode(IHeliumCommandNode node, Dictionary<Int32, Int32> indices)
	{
		return new()
		{
			Flags = FromHeliumFlags(node.Flags),
			ChildrenCount = node.Children.Count(),
			Children = FromHeliumChildren(node.Children, indices),
			RedirectNode = indices[node.Redirect.GetHashCode()],
			Name = node.Command,
			Parser = node.Parser == null ? CommandIdentifiers.Parsers[
				(CommandParserTypes)node.Parser] : null,
			Properties = node.AdditionalProperties,
			SuggestionType = (node.Flags & HeliumCommandNodeFlags.SuggestionsAskServer) != 0 ? 
					CommandIdentifiers.SuggestionBehaviours[CommandSuggestionBehaviourTypes.AskServer] :
				(node.Flags & HeliumCommandNodeFlags.SuggestionsClientside) != 0 ?
					CommandIdentifiers.SuggestionBehaviours[CommandSuggestionBehaviourTypes.AvailableEntities] :
					null
		};
	}

	private static CommandNodeFlags FromHeliumFlags(HeliumCommandNodeFlags flags)
	{
		Byte t = (Byte)((Int32)flags & 0b11);

		if(flags.HasFlag(HeliumCommandNodeFlags.Executable))
		{
			t += (Byte)CommandNodeFlags.Executable;
		}

		if(flags.HasFlag(HeliumCommandNodeFlags.Redirect))
		{
			t += (Byte)CommandNodeFlags.Redirects;
		}

		if(((Int32)flags & 0b1110_0000) != 0)
		{
			t += (Byte)CommandNodeFlags.Suggestions;
		}

		return (CommandNodeFlags)t;
	}

	private static VarInt[] FromHeliumChildren(IEnumerable<IHeliumCommandNode> nodes, Dictionary<Int32, Int32> indices)
	{
		if(nodes == null)
		{
			return null;
		}

		VarInt[] r = new VarInt[nodes.Count()];

		for(Int32 i = 0; i < nodes.Count(); i++)
		{
			r[i] = indices[nodes.ElementAt(i).GetHashCode()];
		}

		return r;
	}
}
