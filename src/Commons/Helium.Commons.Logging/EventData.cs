using System;

namespace Helium.Commons.Logging
{
	/// <summary>
	/// Part of the Helium Toolchain API. Serves as data structure for additional logging data.
	/// </summary>
#nullable enable
	public struct EventData
	{
		internal EventIdentifier identifier { get; set; }
		internal String? name { get; set; }

		/// <summary>
		/// Creates a new EventData instance
		/// </summary>
		/// <param name="PackageId">Numeral, custom package identifier</param>
		/// <param name="NamespaceId">Numeral, custom namespace identifier</param>
		/// <param name="ClassId">Numeral, custom class identifier</param>
		/// <param name="EventId">Numeral, custom method call identifier</param>
		/// <param name="name">Name of the code location the logger is called from</param>
		public EventData(Int16 PackageId, Int16 NamespaceId, Int16 ClassId, Int16 EventId, String? name)
		{
			this.identifier = new EventIdentifier
			{
				PackageId = PackageId,
				NamespaceId = NamespaceId,
				ClassId = ClassId,
				EventId = EventId
			};
			this.name = name;
		}

		/// <summary>
		/// Returns the full Event data string of the given instance
		/// </summary>
		public override String ToString()
		{
			return $"[{this.identifier}/{this.name ?? ""}]";
		}
	}
#nullable disable
}
