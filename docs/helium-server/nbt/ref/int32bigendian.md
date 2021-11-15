# Int32BigEndian

~~~cs
namespace Helium.Nbt.Internal;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct Int32BigEndian
~~~

Represents an `Int32` in big-endian. Used to natively convert large batches of NBT data without any endianness reversal.