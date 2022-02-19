# NbtSByteToken

~~~cs
namespace Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct NbtSByteToken : IValueToken<SByte>
~~~

Represents a 8-bit integer, also used to convey C-style boolean values.

Declarator: `0x01`

## See also

- [IValueToken](../../abstraction/ref/ivaluetoken.md)