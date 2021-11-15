# NbtInt64Token

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtInt64Token : IValuedNbtToken<Int64>
~~~

Represents a NBT token holding one `Int64`.

## Constructors

~~~cs
public NbtInt64Token(Byte[] name, Int64 value)
~~~

Initializes a new `NbtInt64Token` with the specified name and value.