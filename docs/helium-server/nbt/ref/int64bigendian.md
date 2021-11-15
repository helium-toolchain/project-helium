# Int63BigEndian

~~~cs
namespace Helium.Nbt.Internal;

[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct Int64BigEndian
~~~

Represents an `Int64` in big-endian. Used to natively convert large batches of NBT data without any endianness reversal.