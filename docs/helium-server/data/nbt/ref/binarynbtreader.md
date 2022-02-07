# BinaryNbtReader

~~~cs
namespace Helium.Data.Nbt.Serialization;

[RequiresPreviewFeatures]
public sealed class BinaryNbtReader
~~~

This class provides methods for reading binary NBT from a data blob. If possible, it uses `AVX2`, `SSE2`, `SSSE3` or `AdvSimd` hardware intrinsics to speed this procedure up.

## Properties

---

~~~cs
public Int32 MaximumDepth { get; set; } = 512;
~~~

Controls how deep a data structure is allowed to be. The default value of 512 matches the Mojang implementation in this regard.

---

~~~cs
public Boolean ThrowExceptions { get; set; } = true;
~~~

Controls whether exceptions should be thrown upon encountering invalid data. This is ignored in debug builds. If this setting is disabled, the reader will instead return `null`.

## Methods

---

~~~cs
[MethodImpl(MethodImplOptions.AggressiveOptimization)]
public NbtRootToken Deserialize(MemoryStream)
~~~

The central reader method. Contrary to its predecessor library found in commits `29216cf3243df3ab7896894dbd178c29126874ce` and earlier, this method uses a flat parsing loop and is written to use the available hardware as good as it can.

---

~~~cs
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public NbtRootToken Deserialize(String)
~~~

Links to the main reader method, providing convenience by allowing to deserialize a string.

---

~~~cs
[MethodImpl(MethodImplOptions.AggressiveInling)]
public NbtRootToken Deserialize(Byte[])
~~~

Links to the main reader method, providing convenience by allowing to deserialize a byte array.

## See also

- [NbtRootToken](./nbtroottoken)
- [BinaryNbtWriter](./binarynbtwriter)