# NbtInt64ArrayToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtInt64ArrayToken : IValuedComplexNbtToken<Int64>, IList<Int64>
~~~

Represents an array of 64-bit integers.

## Properties

~~~cs
public List<Int64> Elements { get; set; }
~~~

Stores the integers represented by this array token.

## Constructors

~~~cs
public NbtInt64ArrayToken(Byte[] name, List<Int64> values)
~~~

Initializes a new `NbtInt64ArrayToken` with the specified name and values.