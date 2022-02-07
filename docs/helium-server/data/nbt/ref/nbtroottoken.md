# NbtRootToken

~~~cs
namespace Helium.Data.Nbt;

[RequiresPreviewFeatures]
public sealed record NbtRootToken : IRootToken
~~~

This is the utmost root token of any NBT data structure. Functionally equivalent to a `NbtCompoundToken`, this type is required by the abstraction specification.

Declarator: `0x0A`

## See also

- [IRootToken](../../abstraction/ref/iroottoken)
- [NbtCompoundToken](./nbtcompoundtoken)
- [ICompoundToken](../../abstraction/ref/icompoundtoken)