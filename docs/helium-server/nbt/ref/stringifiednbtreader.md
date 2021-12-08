# StringifiedNbtReader

~~~cs
namespace Helium.Nbt.Serialization;

[RequiresPreviewFeatures]
public class StringifiedNbtReader
~~~

Provides a way to read a `NbtCompoundToken` from input data.

## Constructors

~~~cs
public StringifiedNbtReader()
~~~

Creates a new instance.

## Methods

~~~cs
public NbtCompoundToken ReadCompound(String data)
~~~

Reads a compound from a string. Only ASCII characters are valid.

---

~~~cs
public NbtCompoundToken ReadCompound(Byte[] data)
~~~

Reads a compound from a byte array. Only ASCII characters are valid.

## See also

- [`MalformedSNbtException`](./malformedsnbtexception.md)
- [`NbtCompoundToken`](./nbtcompoundtoken.md)