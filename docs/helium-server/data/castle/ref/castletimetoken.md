# CastleTimeToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleTimeToken : ICastleToken, IValueToken<TimeOnly>
~~~

Represents a timestamp without associated date.

Declarator: `0x10`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)