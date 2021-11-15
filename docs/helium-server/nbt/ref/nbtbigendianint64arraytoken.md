# NbtBigEndianInt64ArrayToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtBigEndianInt64ArrayToken : IValuedComplexNbtToken<Int64BigEndian>, IList<Int32BigEndian>
~~~

Represents a big-endian Int64 array. 

## Properties

~~~cs
public List<Int64BigEndian> Elements { get; set; }
~~~

Internally holds all elements of this array token.

## Constructors

~~~cs
public NbtBigEndianInt64ArrayToken(Byte[] name, Span<Int64BigEndian> values)
~~~

Constructs a new `NbtBigEndianInt64ArrayToken` with the specified name and the specified values. The constructor uses a `Span<T>` in order to work with `MemoryMarshal#Cast<TFrom, TTo>`, which this class is intended to be used with.

## See also

- [`Int64BigEndian`](./int64bigendian)