# NbtCompoundToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtCompoundToken : IComplexNbtToken, ICollection
~~~

Represents a NBT compound. This is always the root token of any NBT data blob. Its last child token is always an `NbtEndToken`.

## Constructors

~~~cs
public NbtCompoundToken(Byte[] name, List<INbtToken> children)
~~~

Initializes a new `NbtCompoundToken` with the specified name and children.

---

~~~cs
public NbtCompoundToken()
~~~

Initializes a new nameless `NbtCompoundToken` without children. Used mainly if a root token has to be created.

## See also

- [`NbtEndToken`](./nbtendtoken.md)