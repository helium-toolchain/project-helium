# NbtDoubleToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public record struct NbtDoubleeToken : IValuedNbtToken<Double>
~~~

Represents a NBT token holding one `Double`.

## Constructors

~~~cs
public NbtDoubleToken(Byte[] name, Double value)
~~~

Initializes a new `NbtDoubleToken` with the specified name and value.