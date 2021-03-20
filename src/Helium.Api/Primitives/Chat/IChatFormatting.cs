using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Api.Primitives.Chat
{
	/// <summary>
	/// Extended IChatColour that needs to be handled client-side, allowing some minor performance improvements on both sides.
	/// </summary>
	public interface IChatFormatting : IChatColour
	{
		/// <summary>
		/// False for colours, true for all other chat formattings.
		/// </summary>
		public Boolean IsMarkup { get; }
	}
}
