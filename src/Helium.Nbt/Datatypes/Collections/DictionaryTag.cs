using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helium.Nbt.Datatypes.Collections
{
    public class DictionaryTag<T> : AbstractTag
    {
        public Dictionary<String, T> ValuePairs { get; internal set; }

        internal override AbstractTag Copy()
            => this;

        internal override void Write(Stream writer)
        {
            writer.Write(Encoding.UTF8.GetBytes($"{Key}: {{"));
            foreach(var v in ValuePairs)
                writer.Write(Encoding.UTF8.GetBytes($", {v.Key}: {v.Value}"));
            writer.Write(Encoding.UTF8.GetBytes($"}}"));
        }
    }
}
