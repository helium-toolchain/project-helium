# NbtSingleToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtSingleToken : IValuedNbtToken<Single>
~~~

Represents a NBT token holding one `Single`.

## Constructors

~~~cs
public NbtSingleToken(Byte[] name, Single value)
~~~

Initializes a new `NbtSingleToken` with the specified name and value.