namespace Helium.Api.Mojang;

using System;

/// <summary>
/// Controls the five different ways a client can handle autocomplete for a command node.
/// </summary>
public enum CommandSuggestionBehaviourTypes : Byte
{
	/// <summary>
	/// Sends the S0x06 Tab-Complete packet to the server to ask for tab-complete help
	/// </summary>
	AskServer,

	/// <summary>
	/// All available recipes. <c>minecraft:all_recipes</c> in the protocol.
	/// </summary>
	AvailableRecipes,

	/// <summary>
	/// All available sounds.
	/// </summary>
	AvailableSounds,

	/// <summary>
	/// All available biomes.
	/// </summary>
	AvailableBiomes,

	/// <summary>
	/// All available entities. <c>minecraft:summonable_entities</c> in the protocol.
	/// </summary>
	AvailableEntities
}
