using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helium.Nbt.Datatypes
{
    public class CompoundTag : AbstractTag
    {
        public List<AbstractTag> Values { get; internal set; }

        internal override AbstractTag Copy()
            => this;

        internal override void Write(Stream writer)
        {
            foreach(var v in Values)
            {
                v.Write(writer);
                writer.Write(Encoding.UTF8.GetBytes(", "));
            }
        }
    }
}
