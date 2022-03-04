# CastleGuidToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleGuidToken : ICastleToken, IValueToken<Guid>
~~~

Represents a GUID.

Declarator: `0x1C`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)