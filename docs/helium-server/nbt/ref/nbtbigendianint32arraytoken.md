# NbtBigEndianInt32ArrayToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtBigEndianInt32ArrayToken : IValuedComplexNbtToken<Int32BigEndian>, IList<Int32BigEndian>
~~~

Represents a big-endian Int32 array. 

## Properties

~~~cs
public List<Int32BigEndian> Elements { get; set; }
~~~

Internally holds all elements of this array token.

## Constructors

~~~cs
public NbtBigEndianInt32ArrayToken(Byte[] name, Span<Int32BigEndian> values)
~~~

Constructs a new `NbtBigEndianInt32ArrayToken` with the specified name and the specified values. The constructor uses a `Span<T>` in order to work with `MemoryMarshal#Cast<TFrom, TTo>`, which this class is intended to be used with.

## See also

- [`Int32BigEndian`](./int32bigendian)