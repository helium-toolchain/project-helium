namespace Helium.Api.Primitives.Item;

/// <summary>
/// Mock item used to construct the actual item.
/// </summary>
public interface IHeliumServerMockItem
{
	/// <summary>
	/// Registry-assigned Id matching the one assigned to the corresponding IHeliumServerItem
	/// </summary>
	public Int64 Id { get; set; }
}
