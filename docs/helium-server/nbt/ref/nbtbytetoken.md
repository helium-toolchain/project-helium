# NbtByteToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtByteToken : IValuedNbtToken<Byte>
~~~

Represents a NBT token holding one byte.

## Constructors

~~~cs
public NbtByteToken(Byte[] name, Byte value)
~~~

Initializes a new `NbtByteToken` with the specified name and value.