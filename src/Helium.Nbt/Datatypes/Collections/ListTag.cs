using System;
using System.Collections.Generic;
using System.Text;

namespace Helium.Nbt.Datatypes.Collections
{
    public class ListTag<T> : AbstractEnumerableTag<T, List<T>>
        where T : AbstractTag
    {
    }
}
