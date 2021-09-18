namespace Helium.Api.Primitives.Item;

using System;

using Helium.Api.Primitives.Chat;

/// <summary>
/// Base interface for all Helium Items. Contains all networking relevant information used by Helium.Network.
/// Core implementations are recommended to use their own Item base interface to further extend on this
/// </summary>
public interface IHeliumServerItem
{
	/// <summary>
	/// ID of the item. Only important for handling clients implementing the Helium network protocol.
	/// This should never be hardcoded or assigned manually, instead a Core implemenatation should implement a item
	/// registry to properly ensure mod consistency.
	/// </summary>
	public Int64 Id { get; }

	/// <summary>
	/// String name of the namespace this item is defined in. Only important for handling clients implementing the 
	/// Notchian network protocol. Per convention, this uses snake case.
	/// </summary>
	public String Namespace { get; }

	/// <summary>
	/// String name of the item. Only important for handling clients implementing the Notchian network protocol.
	/// Per convention, this uses snake case.
	/// </summary>
	public String ItemName { get; }

	/// <summary>
	/// Formatting for this item, used in chat and for tooltips.
	/// </summary>
	public IChatFormatting Formatting { get; }

	/// <summary>
	/// Additional data for this item: display name, enchantments etc.
	/// </summary>
	public String AdditionalData { get; }
}
