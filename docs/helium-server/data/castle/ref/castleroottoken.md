# CastleRootToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public sealed record CastleRootToken : ICastleParentToken, IRootToken
~~~

This is the utmost root token of any Castle data structure. This type also holds the array of all names used in the data structure.

Declarator: `0x00`

## See also

- [IRootToken](../../abstraction/ref/iroottoken.md)
- [CastleCompoundToken](./castlecompoundtoken.md)
- [ICompoundToken](../../abstraction/ref/icompoundtoken.md)