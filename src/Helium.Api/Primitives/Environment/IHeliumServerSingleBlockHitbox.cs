using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Api.Primitives.Environment
{
	/// <summary>
	/// Singular hitbox. A block hitbox can be composed of multiple of these.
	/// Neither of the contained values can ever exceed 1, Core implementations should make sure of that.
	/// The Helium network protocol allows hitboxes to overlap, the notchian protocol does not.
	/// <para>
	///		Start coordinates should be the lower coordinates, End coordinates the higher coordinates.
	///		All coordinates are relative to the block and cannot exceed 1. Coordinates are counted from NegativeInfinity.
	/// </para>
	/// </summary>
	public interface IHeliumServerSingleBlockHitbox
	{
		/// <summary>
		/// X coordinate of the hitbox start, relative to the block.
		/// </summary>
		public Single XStart { get; set; }

		/// <summary>
		/// Y coordinate of the hitbox start, relative to the block.
		/// </summary>
		public Single YStart { get; set; }

		/// <summary>
		/// Z coordinate of the hitbox start, relative to the block.
		/// </summary>
		public Single ZStart { get; set; }

		/// <summary>
		/// X coordinate of the hitbox end, relative to the block.
		/// </summary>
		public Single XEnd { get; set; }

		/// <summary>
		/// Y coordinate of the hitbox end, relative to the block
		/// </summary>
		public Single YEnd { get; set; }

		/// <summary>
		/// Z coordinate of the hitbox end, relative to the block
		/// </summary>
		public Single ZEnd { get; set; }
	}
}
