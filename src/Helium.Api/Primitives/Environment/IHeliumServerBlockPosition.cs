using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Api.Primitives.Environment
{
	public interface IHeliumServerBlockPosition
	{
		public Int32 X { get; }
		public Int32 Y { get; }
		public Int32 Z { get; }

		public Int32 NotchianX();
		public Int32 NotchianY();
		public Int32 NotchianZ();
	}
}
