# BinaryNbtWriter

~~~cs
namespace Helium.Data.Nbt.Serialization;

[RequiresPreviewFeatures]
public sealed class BinaryNbtWriter
~~~

Provides a mechanism to write NBT data to binary. This class is not as powerfully optimized as `BinaryNbtReader`, as writing can take place asynchronously much more commonly.

## Methods

---

~~~cs
public Byte[] Serialize(NbtRootToken)
~~~

The main serializer.

## See also

- [NbtRootToken](./nbtroottoken.md)
- [BinaryNbtReader](./binarynbtreader.md)