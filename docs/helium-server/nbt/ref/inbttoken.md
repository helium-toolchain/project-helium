# INbtToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public interface INbtToken
~~~

`INbtToken` serves as base interface for all NBT tokens. It forces the specific token implementation to expose common data required for all tokens.

# Properties

~~~cs
public abstract static Byte Declarator { get; }
~~~

Declares the NBT binary type declarator; the byte in the NBT stream that indicates the token datatype.

---

~~~cs
public abstract static Byte Length { get; }
~~~

Declares the binary length of the value of this token. 0 indicates a variable- or zero-length token.

---

~~~cs
public Byte[] Name { get; }
~~~

Declares the name of this token. May be ommitted for the root `NbtCompoundToken` or tokens inside a `NbtListToken`.