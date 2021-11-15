# BinaryNbtReader

~~~cs
namespace Helium.Nbt.Serialization;

[RequiresPreviewFeatures]
public class BinaryNbtReader
~~~

Provides logic for reading bNBT from a stream.

## Properties

~~~cs
public Stream DataStream { get; set; }
~~~

Represents the stream data is read from.

## Methods

~~~cs
public NbtCompoundToken ReadCompound()
~~~

Reads a `NbtCompoundToken` from the current stream.

## Constructors

~~~cs
public BinaryNbtReader(Stream input, BinaryReaderEndianness endianness)
~~~

Creates a new `BinaryNbtReader` with the specified input stream and the specified `BinaryReaderEndianness`.

## See also

- [`NbtCompoundToken`](./nbtcompoundtoken)
- [`BinaryReaderEndianness`](./binaryreaderendianness)
- [`BinaryNbtWriter`](./binarynbtwriter)