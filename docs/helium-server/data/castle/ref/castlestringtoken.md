# CastleStringToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleStringToken : ICastleToken, IValueToken<String>
~~~

Represents a string, serialized to disk in UTF-8.

Declarator: `0x0C`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)