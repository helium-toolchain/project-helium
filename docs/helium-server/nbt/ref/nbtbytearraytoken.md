# NbtByteArrayToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtByteArrayToken : IValuedComplexNbtToken<Byte>, IList<Byte>
~~~

Represents an array of bytes.

## Properties

~~~cs
public List<Byte> Elements { get; set; }
~~~

Stores the bytes represented by this array token.

## Constructors

~~~cs
public NbtByteArrayToken(Byte[] name, Span<Byte> values)
~~~

Initializes a new `NbtByteArrayToken` with the specified name and values.