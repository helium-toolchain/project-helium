using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Api.Primitives.Chat
{
	/// <summary>
	/// Describes a colour gradient. This is unused by the Vanilla Helium Server and
	/// unavailable for versions 1.15.2 and lower of the Notchian protocol. The inherited colour
	/// is used for those versions of the Notchian protocol. Additionally, versions 1.16.0 and higher
	/// of the Notchian protocol do not support gradient breakpoints. For those, only the first
	/// colour point is used.
	/// </summary>
	public interface IChatColourGradient : IChatColour
	{
		/// <summary>
		/// Colour gradient points associated with the breakpoints
		/// </summary>
		public Int32[] ColourPoints { get; set; }

		/// <summary>
		/// Colour breakpoints indicating where in the text the gradient should change.
		/// </summary>
		public Single[] GradientBreakpoints { get; set; }
	}
}
