# NbtStringToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public sealed class NbtStringToken : IValuedNbtToken<String>
~~~

Represents a NBT token holding one `String`.

## Constructors

~~~cs
public NbtStringToken(Byte[] name, String value)
~~~

Initializes a new `NbtStringToken` with the specified name and value.

---

~~~cs
public NbtStringToken(Byte[] name, Span<Byte> value)
~~~

Initializes a new `NbtStringToken` with the specified name and value.