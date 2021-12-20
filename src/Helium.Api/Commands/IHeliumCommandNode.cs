namespace Helium.Api.Commands;

using System;
using System.Collections.Generic;

using Helium.Api.Mojang;

/// <summary>
/// This serves as a base interface for command nodes specified using Helium.
/// </summary>
public interface IHeliumCommandNode
{
#nullable enable
	/// <summary>
	/// Flags applied to this command node.
	/// </summary>
	public HeliumCommandNodeFlags Flags { get; set; }

	/// <summary>
	/// Parent node. Can be <see langword="null"/> if this is the root node.
	/// </summary>
	public IHeliumCommandNode? Parent { get; set; }

	/// <summary>
	/// Redirect node. <see langword="null"/> if this node does not redirect.
	/// </summary>
	public IHeliumCommandNode? Redirect { get; set; }

	/// <summary>
	/// Child nodes. Need not contain anything.
	/// </summary>
	public IEnumerable<IHeliumCommandNode>? Children { get; set; }


	/// <summary>
	/// Parser used to parse this node.
	/// </summary>
	public CommandParserTypes? Parser { get; set; }

	/// <summary>
	/// Additional data for the parser used to parse this node. Only set if this node is parsable.
	/// </summary>
	public ICommandParserProperties? AdditionalProperties { get; set; }

	/// <summary>
	/// Command literal or argument name.
	/// </summary>
	public String Command { get; set; }

	/// <inheritdoc/>
	public Int32 GetHashCode()
	{
		return Command.GetHashCode();
	}
#nullable restore
}
