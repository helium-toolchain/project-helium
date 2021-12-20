namespace Helium.Api.Mojang;

/// <summary>
/// Specifies all valid command parser types accepted by the vanilla client.
/// </summary>
public enum CommandParserTypes
{
	/// <summary>
	/// Boolean value, <c>true</c> or <c>false</c>, case-sensitive.
	/// </summary>
	Boolean,

	/// <summary>
	/// Double-precision floating-point number.
	/// </summary>
	Double,

	/// <summary>
	/// Single-precision floating-point number.
	/// </summary>
	Single,

	/// <summary>
	/// 32-bit integer.
	/// </summary>
	Int32,

	/// <summary>
	/// 64-bit integer.
	/// </summary>
	Int64,

	/// <summary>
	/// A string with a maximum length of 32767.
	/// </summary>
	String,

	/// <summary>
	/// A selector, player name or entity UUID
	/// </summary>
	Entity,

	/// <summary>
	/// A player, online or not; or a selector matching one or more players.
	/// </summary>
	Player,

	/// <summary>
	/// A block location, represented as three integers. May use relative locations.
	/// </summary>
	BlockPosition,

	/// <summary>
	/// A column location, represented as two integers. May use relative locations.
	/// </summary>
	ColumnPosition,

	/// <summary>
	/// A location, represented as three decimal numbers. May use relative locations.
	/// </summary>
	Vec3,

	/// <summary>
	/// A location, repesented as two decimal numbers. May use relative locations.
	/// </summary>
	Vec2,

	/// <summary>
	/// A block state, optionally including state information or NBT if this is a block entity.
	/// </summary>
	Blockstate,

	/// <summary>
	/// A block or block tag.
	/// </summary>
	BlockPredicate,

	/// <summary>
	/// An item, optionally including NBT.
	/// </summary>
	ItemStack,

	/// <summary>
	/// An item or item tag.
	/// </summary>
	ItemPredicate,

	/// <summary>
	/// A chat color or <c>reset</c>. Case-insensitive.
	/// </summary>
	Color,

	/// <summary>
	/// A JSON chat component.
	/// </summary>
	Component,

	/// <summary>
	/// A chat message, potentially including selectors.
	/// </summary>
	Message,

	/// <summary>
	/// A sNBT value.
	/// </summary>
	Nbt,

	/// <summary>
	/// A path within a sNBT value.
	/// </summary>
	NbtPath,

	/// <summary>
	/// A scoreboard objective.
	/// </summary>
	Objective,

	/// <summary>
	/// A single score criterion.
	/// </summary>
	ObjectiveCriterion,

	/// <summary>
	/// A scoreboard operator.
	/// </summary>
	Operation,

	/// <summary>
	/// A particle effect, mirroring the Particle protocol entity.
	/// </summary>
	Particle,

	/// <summary>
	/// An angle, represented as two decimal numbers. May use relative locations.
	/// </summary>
	Rotation,

	/// <summary>
	/// A singular angle, represented as one decimal number. May use relative locations.
	/// </summary>
	Angle,

	/// <summary>
	/// A scoreboard display position slot.
	/// </summary>
	ScoreboardSlot,

	/// <summary>
	/// Anything that can join a team. Allows selectors and wildcards.
	/// </summary>
	ScoreHolder,

	/// <summary>
	/// A collection of up to three axes.
	/// </summary>
	Swizzle,

	/// <summary>
	/// A team name.
	/// </summary>
	Team,

	/// <summary>
	/// An inventory slot.
	/// </summary>
	InventorySlot,

	/// <summary>
	/// Any valid identifier.
	/// </summary>
	Identifier,

	/// <summary>
	/// Any status effect.
	/// </summary>
	StatusEffect,

	/// <summary>
	/// A datapack-declared function.
	/// </summary>
	Function,

	/// <summary>
	/// The entity anchor, feet or eyes.
	/// </summary>
	EntityAnchor,

	/// <summary>
	/// A range of values with a minimum and a maximum value.
	/// </summary>
	Range,

	/// <summary>
	/// A range of integers with a minimum and a maximum value.
	/// </summary>
	Int32Range,

	/// <summary>
	/// A range of decimals with a minimum and a maximum value.
	/// </summary>
	SingleRange,

	/// <summary>
	/// An enchantment.
	/// </summary>
	Enchantment,

	/// <summary>
	/// An entity summon.
	/// </summary>
	EntitySummon, //todo: find out whatever the hell this actually is

	/// <summary>
	/// A dimension.
	/// </summary>
	Dimension,

	/// <summary>
	/// A UUID.
	/// </summary>
	Uuid,

	/// <summary>
	/// A partial sNBT token.
	/// </summary>
	NbtToken,

	/// <summary>
	/// A complete sNBT token.
	/// </summary>
	NbtCompoundToken,

	/// <summary>
	/// A time duration.
	/// </summary>
	Time
}
