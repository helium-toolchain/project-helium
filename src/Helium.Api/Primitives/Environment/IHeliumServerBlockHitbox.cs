using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Api.Primitives.Environment
{
	/// <summary>
	/// Base interface for block hitboxes.
	/// </summary>
	public interface IHeliumServerBlockHitbox
	{
		/// <summary>
		/// Sub-Hitboxes this hitbox consists of. 
		/// For the Helium network protocol, these can overlap; for the Notchian network protocol these cannot overlap.
		/// </summary>
		public IHeliumServerSingleBlockHitbox[] SubHitboxes { get; set; }
	}
}
