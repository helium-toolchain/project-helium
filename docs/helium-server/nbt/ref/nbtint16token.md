# NbtInt16Token

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtInt16Token : IValuedNbtToken<Int16>
~~~

Represents a NBT token holding one `Int16`.

## Constructors

~~~cs
public NbtInt16Token(Byte[] name, Int16 value)
~~~

Initializes a new `NbtInt16Token` with the specified name and value.