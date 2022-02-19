# CastleDoubleToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleDoubleToken : ICastleToken, IValueToken<Double>
~~~

Represents an 64-bit IEEE754 floating-point number.

Declarator: `0x0B`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)