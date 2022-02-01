# StringifiedNbtReader

~~~cs
namespace Helium.Data.Nbt.Serialization;

[RequiresPreviewFeatures]
public sealed class StringifiedNbtReader
~~~

Provides a mechanism to read a `NbtRootToken` from a sNBT string.

## Methods

---

~~~cs
public NbtRootToken Deserialize(String)
~~~

Deserializes the given string according to the sNBT specification.

## See also

- [NbtRootToken](./nbtroottoken)
- [StringifiedNbtWriter](./stringifiednbtwriter)