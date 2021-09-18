namespace Helium.Api.Primitives.Environment;

using System;

using Helium.Api.Primitives.Item;

/// <summary>
/// Base interface for all Helium Blocks. Contains all networking relevant information used by Helium.Network.
/// Core implementations are recommended to use their own Block base interface to further extend on this.
/// </summary>
public interface IHeliumServerBlock
{
	/// <summary>
	/// String name of the namespace this block is defined in. Only important for handling clients implementing the 
	/// Notchian network protocol. Per convention, this uses snake case.
	/// </summary>
	public static String Namespace { get; }

	/// <summary>
	/// String name of the block. Only important for handling clients implementing the Notchian network protocol.
	/// Per convention, this uses snake case.
	/// </summary>
	public static String BlockName { get; }

	/// <summary>
	/// ID of the block. Only important for handling clients implementing the Helium network protocol.
	/// This should never be hardcoded or assigned manually, instead a Core implemenatation should implement a block
	/// registry to properly ensure mod consistency.
	/// </summary>
	public static Int64 Id { get; set; }

	/// <summary>
	/// Current position of the block. If a block is moved, we delete the existing instance and create a new one - no setter!
	/// </summary>
	public IHeliumServerBlockPosition Position { get; }

	/// <summary>
	/// Currently active block states. Inactive block states should not be stored here to improve network performance.
	/// </summary>
	public IHeliumServerBlockState[] BlockStates { get; set; }

	/// <summary>
	/// Current block hitbox. This may change whenever - it's on the client to respect changes.
	/// </summary>
	public IHeliumServerBlockHitbox Hitbox { get; set; }

	/// <summary>
	/// Current block collision box. This may change whenever - it's on the client to respect changes.
	/// </summary>
	public IHeliumServerBlockCollisionBox CollisionBox { get; set; }

	/// <summary>
	/// Current light level emitted by the block, calculated from the bottom face of the block.
	/// The light level directly beside it would be lower by one.
	/// </summary>
	public Byte LightLevel { get; set; }


	/// <summary>
	/// Total list of all allowed block states for this block. They can be conflicting, this needs
	/// to be handled within the Core implementation.
	/// </summary>
	public static IHeliumServerBlockState[] AllowedBlockStates { get; }

	/// <summary>
	/// Item form of this block, important for various exploits a Core implementation should implement
	/// and for communicating with clients using the Notchian network protocol.
	/// </summary>
	public static IHeliumServerMockItem Item { get; }

	/// <summary>
	/// Block hardness, solely used for mining. Same measurement as the Notchian server.
	/// </summary>
	public static Single Hardness { get; }

	/// <summary>
	/// Block blast resistance. Same measurement as the Notchian server.
	/// </summary>
	public static Single BlastResistance { get; }

	/// <summary>
	/// Jump height multiplier when walking on this block. Same measurement as the Notchian server.
	/// </summary>
	public static Single JumpMultiplier { get; } // can vanilla stop requiring completely useless data via network?
}
