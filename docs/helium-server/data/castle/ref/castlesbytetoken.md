# CastleSByteToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleSByteToken : ICastleToken, IValueToken<SByte>
~~~

Represents an 8-bit signed integer, also used to convey C-style booleans.

Declarator: `0x02`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)