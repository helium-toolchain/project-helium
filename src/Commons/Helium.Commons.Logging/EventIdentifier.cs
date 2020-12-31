using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helium.Commons.Logging
{
    internal struct EventIdentifier
    {
        internal Int16 PackageId { get; init; }
        internal Int16 NamespaceId { get; init; }
        internal Int16 ClassId { get; init; }
        internal Int16 EventId { get; init; }

        internal EventIdentifier(Int16 PackageId = 0, Int16 NamespaceId= 0, Int16 ClassId = 0, Int16 EventId = 0)
        {
            this.PackageId = PackageId;
            this.NamespaceId = NamespaceId;
            this.ClassId = ClassId;
            this.EventId = EventId;
        }

        public override Boolean Equals(Object obj) 
            => base.Equals(obj);

        public override Int32 GetHashCode() 
            => base.GetHashCode();

        /// <summary>
        /// Returns the string translation of this EventIdentifier
        /// </summary>
        public override String ToString() 
            => $"{PackageId}.{NamespaceId}.{ClassId}.{EventId}";

        public static Boolean operator == (EventIdentifier left, EventIdentifier right)
        {
            return (left.PackageId == right.PackageId) && (left.NamespaceId == right.NamespaceId) &&
                (left.ClassId == right.ClassId) && (left.EventId == right.EventId);
        }

        public static Boolean operator != (EventIdentifier left, EventIdentifier right)
        {
            return !(left == right);
        }
    }
}
