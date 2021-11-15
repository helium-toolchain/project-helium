# BinaryReaderEndianness

~~~cs
namespace Helium.Nbt.Serialization;

public enum BinaryReaderEndianness
~~~

Defines the two different deserialization handling modes for Int32 and Int64 arrays.

## Fields

~~~cs
Native
~~~

Use the system default endianness (little-endian). Considerably slower reading and writing; but `BinaryNbtReader` handles conversion to little-endian for you.

---

~~~cs
BigEndian
~~~

Force big-endian; causing `BinaryNbtReader` to use `Int32BigEndian` and `Int64BigEndian` over `Int32` and `Int64`, respectively. Significantly faster reading and writing, but `BinaryNbtReader` handles no conversion for you.

## See also

- [`BinaryNbtReader`](./binarynbtreader)
- [`BinaryNbtWriter`](./binarynbtwriter)
- [`Int32BigEndian`](./int32bigendian)
- [`Int64BigEndian`](./int64bigendian)