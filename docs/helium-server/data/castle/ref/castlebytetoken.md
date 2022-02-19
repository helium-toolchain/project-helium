# CastleByteToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleByteToken : ICastleToken, IValueToken<Byte>
~~~

Represents an 8-bit unsigned integer, also used to convey C-style booleans.

Declarator: `0x01`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)