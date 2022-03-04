# CastleString16Token

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public record struct CastleString16Token : ICastleToken, IValueToken<String16>
~~~

Represents a string, serialized to disk as UTF16.

Declarator: `0x0D`

## See also

- [ICastleToken](./icastletoken.md)
- [IValueToken](../../abstraction/ref/ivaluetoken.md)