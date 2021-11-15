# NbtInt32ArrayToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtInt32ArrayToken : IValuedComplexNbtToken<Int32>, IList<Int32>
~~~

Represents an array of 32-bit integers.

## Properties

~~~cs
public List<Int32> Elements { get; set; }
~~~

Stores the integers represented by this array token.

## Constructors

~~~cs
public NbtInt32ArrayToken(Byte[] name, List<Int32> values)
~~~

Initializes a new `NbtInt32ArrayToken` with the specified name and values.