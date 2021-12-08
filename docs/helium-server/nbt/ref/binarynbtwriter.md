# BinaryNbtWriter

~~~cs
namespace Helium.Nbt.Serialization;

[RequiresPreviewFeatures]
public class BinaryNbtWriter
~~~

Provides logic for writing bNBT to a stream.

## Properties

~~~cs
public Stream DataStream { get; set; }
~~~

Represents the stream data is written to.

## Methods

~~~cs
public void WriteCompound(NbtCompoundToken root)
~~~

Writes a `NbtCompoundToken` to the current stream.

## Constructors

~~~cs
public BinaryNbtReader(Stream dataStream)
~~~

Creates a new `BinaryNbtReader` with the specified output stream.

## See also

- [`NbtCompoundToken`](./nbtcompoundtoken.md)
- [`BinaryNbtReader`](./binarynbtreader.md)