using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging
{
    public struct EventData
    {
        internal EventIdentifier identifier { get; set; }
        internal String name { get; set; }

        public EventData(Int16 PackageId, Int16 NamespaceId, Int16 ClassId, Int16 EventId, String name)
        {
            identifier = new EventIdentifier
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
            => $"[{identifier}/{name}]";
    }
}
