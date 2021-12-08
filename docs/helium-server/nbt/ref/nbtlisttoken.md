# NbtListToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtListToken : IValuedComplexNbtToken<INbtToken>, IList<INbtToken>, ITypelessList
~~~

Represents a list of any nameless NBT token.

`BinaryNbtReader#ReadCompound()` will still assign all tokens a name, but those names can be neglected and are not written by `BinaryNbtWriter#WriteCompound(NbtCompoundToken root)`.

---

Helium.Nbt does not strictly type lists in-memory; however, they will always be read and written as one type.

## Properties

~~~cs
public List<INbtToken> Content { get; set; }
~~~

Represents the contents of this list. 

---

~~~cs
public Int32 TargetLength { get; set; }
~~~

Gets the full length of this list when read or written to bNBT. Any consumer changing `Content` should also change `TargetLength`.

---

~~~cs
public NbtTokenType ListTokenType { get; set; }
~~~

Gets the `NbtTokenType` of this list.

## Constructors

~~~cs
public NbtListToken(Byte[] name, List<INbtToken> content, Int32 targetLength, NbtTokenType tokenType)
~~~

Initializes a new `NbtListToken` with the specified properties.

## See also

- [`NbtTokenType`](./nbttokentype.md)