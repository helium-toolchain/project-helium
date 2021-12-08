# StringifiedNbtWriter

~~~cs
namespace Helium.Nbt.Serialization;

[RequiresPreviewFeatures]
public class StringifiedNbtWriter
~~~

Provides methods for writing a `NbtCompoundToken` to stringified NBT.

## Constructors

~~~cs
public StringifiedNbtWriter()
~~~

Creates a new instance.

## Methods

~~~cs
public Byte[] WriteCompound(NbtCompoundToken compound)
~~~

Writes the given compound token to a sNBT ASCII Byte array.