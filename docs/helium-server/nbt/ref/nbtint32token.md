# NbtInt32Token

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtInt32Token : IValuedNbtToken<Int32>
~~~

Represents a NBT token holding one `Int32`.

## Constructors

~~~cs
public NbtInt32Token(Byte[] name, Int32 value)
~~~

Initializes a new `NbtInt32Token` with the specified name and value.