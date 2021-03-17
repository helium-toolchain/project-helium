using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Core.World.Environment.Map.MapColors 
{
	public record MapColor 
	{
		public UInt16 Id { get; init; }
		public UInt32 Color { get; init; }
	}
}
