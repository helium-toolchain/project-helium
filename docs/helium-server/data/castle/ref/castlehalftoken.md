# CastleHalfToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleHalfToken : ICastleToken, IValueToken<Half>
~~~

Represents an 16-bit IEEE754 floating-point number.

Declarator: `0x09`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)