namespace Helium.Api.Mojang;

using System;
using System.Collections.Generic;

public static class CommandIdentifiers
{
	public static readonly Dictionary<CommandParserTypes, String> Parsers = new()
	{
		[CommandParserTypes.Boolean] = "brigadier:bool",
		[CommandParserTypes.Double] = "brigadier:double",
		[CommandParserTypes.Single] = "brigadier:float",
		[CommandParserTypes.Int32] = "brigadier:integer",
		[CommandParserTypes.Int64] = "brigadier:long",
		[CommandParserTypes.String] = "brigadier:string",
		[CommandParserTypes.Entity] = "minecraft:entity",
		[CommandParserTypes.Player] = "minecraft:game_profile",
		[CommandParserTypes.BlockPosition] = "minecraft:block_pos",
		[CommandParserTypes.ColumnPosition] = "minecraft:column_position",
		[CommandParserTypes.Vec3] = "minecraft:vec3",
		[CommandParserTypes.Vec2] = "minecraft:vec2",
		[CommandParserTypes.Blockstate] = "minecraft:block_state",
		[CommandParserTypes.BlockPredicate] = "minecraft:block_predicate",
		[CommandParserTypes.ItemStack] = "minecraft:item_stack",
		[CommandParserTypes.ItemPredicate] = "minecraft:item_predicate",
		[CommandParserTypes.Color] = "minecraft:color",
		[CommandParserTypes.Component] = "minecraft:component",
		[CommandParserTypes.Message] = "minecraft:message",
		[CommandParserTypes.Nbt] = "minecraft:nbt",
		[CommandParserTypes.NbtPath] = "minecraft:nbt_path",
		[CommandParserTypes.Objective] = "minecraft:objective",
		[CommandParserTypes.ObjectiveCriterion] = "minecraft:objective_criteria",
		[CommandParserTypes.Operation] = "minecraft:operation",
		[CommandParserTypes.Particle] = "minecraft:particle",
		[CommandParserTypes.Rotation] = "minecraft:rotation",
		[CommandParserTypes.Angle] = "minecraft:angle",
		[CommandParserTypes.ScoreboardSlot] = "minecraft:scoreboard_slot",
		[CommandParserTypes.ScoreHolder] = "minecraft:score_holder",
		[CommandParserTypes.Swizzle] = "minecraft:swizzle",
		[CommandParserTypes.Team] = "minecraft:team",
		[CommandParserTypes.InventorySlot] = "minecraft:item_slot",
		[CommandParserTypes.Identifier] = "minecraft:resource_location",
		[CommandParserTypes.StatusEffect] = "minecraft:mob_effect",
		[CommandParserTypes.Function] = "minecraft:function",
		[CommandParserTypes.EntityAnchor] = "minecraft:entity_anchor",
		[CommandParserTypes.Range] = "minecraft:range",
		[CommandParserTypes.Int32Range] = "minecraft:int_range",
		[CommandParserTypes.SingleRange] = "minecraft:float_range",
		[CommandParserTypes.Enchantment] = "minecraft:item_enchantment",
		[CommandParserTypes.EntitySummon] = "minecraft:entity_summon",
		[CommandParserTypes.Dimension] = "minecraft:dimension",
		[CommandParserTypes.Uuid] = "minecraft:uuid",
		[CommandParserTypes.NbtToken] = "minecraft:nbt_tag",
		[CommandParserTypes.NbtCompoundToken] = "minecraft:nbt_compound_tag",
		[CommandParserTypes.Time] = "minecraft:time"
	};

	public static readonly Dictionary<CommandSuggestionBehaviourTypes, String> SuggestionBehaviours = new()
	{
		[CommandSuggestionBehaviourTypes.AskServer] = "minecraft:ask_server",
		[CommandSuggestionBehaviourTypes.AvailableRecipes] = "minecraft:available_recipes",
		[CommandSuggestionBehaviourTypes.AvailableSounds] = "minecraft:available_sounds",
		[CommandSuggestionBehaviourTypes.AvailableBiomes] = "minecraft:available_biomes",
		[CommandSuggestionBehaviourTypes.AvailableEntities] = "minecraft:summonable_entities"
	};
}
