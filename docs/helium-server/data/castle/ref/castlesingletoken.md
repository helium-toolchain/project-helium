# CastleSingleToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleSingleToken : ICastleToken, IValueToken<Single>
~~~

Represents an 32-bit IEEE754 floating-point number.

Declarator: `0x0A`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)