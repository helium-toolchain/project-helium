namespace Helium.Api.Primitives.Environment;

/// <summary>
/// Secondary hitbox, for collisions rather than interactions
/// </summary>
public interface IHeliumServerBlockCollisionBox
{
	/// <summary>
	/// Sub-Hitboxes this hitbox consists of. 
	/// For the Helium network protocol, these can overlap; for the Notchian network protocol these cannot overlap.
	/// </summary>
	public IHeliumServerSingleBlockHitbox[] SubHitboxes { get; set; }
}
