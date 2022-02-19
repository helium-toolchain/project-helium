# CastleDateToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleDateToken : ICastleToken, IValueToken<DateOnly>
~~~

Represents a date.

Declarator: `0x0F`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)