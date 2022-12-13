namespace Helium.Api.Primitives.Item;

using System;

using Helium.Api.Primitives.Chat;

/// <summary>
/// Item rarity base interface. In vanilla, item rarity is only cosmetic.
/// </summary>
public interface IItemRarity
{
	/// <summary>
	/// Translation name of the item rarity. Only used for the Helium network protocol.
	/// </summary>
	public String Name { get; }

	/// <summary>
	/// Item rarity Id, should be assigned by a registry system. The four vanilla rarity values should be hardcoded.
	/// </summary>
	public Int16 Id { get; }

	/// <summary>
	/// Chat format rules that should be used to display this item's name.
	/// </summary>
	public IChatFormatting ChatFormatting { get; set; }
}
