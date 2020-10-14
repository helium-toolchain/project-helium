using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helium.Nbt.Datatypes.Collections
{
    public abstract class AbstractEnumerableTag<Type, Enumerable> : AbstractTag
        where Type : AbstractTag
        where Enumerable : IEnumerable<Type>
    {
        public Enumerable Values { get; internal set; }

        internal override AbstractTag Copy()
            => this;

        internal override void Write(Stream writer)
        {
            writer.Write(Encoding.UTF8.GetBytes("["));
            foreach (var v in Values)
                v.Write(writer);
            writer.Write(Encoding.UTF8.GetBytes("]"));
        }
    }
}
