# Castle

Castle is the Helium Projects own serialization format. It is designed to be fast, consistent and versatile and to interoperate seamlessly with Crossroads. It is mostly optimized for parsing and takes some trade-offs for writing, since writing is often less performance-critical than reading. It is not necessarily optimized for disk space and prefers fast reading over compressing data as much as possible.

## Differences from the NBT format

1. Castle stores data in little-endian as opposed to big endian, conforming with the native architecture of every platform Helium runs on.
2. Castle uses 16-bit integers for every length declaration instead of using a mixture of 16-bit and 32-bit integers.
3. Castle implements unsigned integers and Half-precision floating point types.
4. Castle declares total lengths for each token to allow for faster indexing.
5. Castle deduplicates all token names.

## Specification

Each Castle token is built in the following structure:

> token ID: 1 byte
> 
> (total length: 4 bytes)
>
> name deduplication ID: 2 bytes
> 
> payload

The total length field is omitted for primitive tokens (0x01 to 0x1C) as their length is already known.

Each Castle file is wrapped into a compound token, the so-called Root token, which should also contain all names, indexed to their deduplication IDs. This root token should declare the total length of its string deduplication array as its total length; opening each file with `00 (root tokens are declared as 0x00) xxxxxxxx (total length of the names) 0000 (skipping name deduplication)`, following by the name array and the payload. Any file starting differently does not conform to the specification.

Names can only consist of ASCII characters.

### Valid token types

Name | .NET name | ID | Payload length | Notes
---- | --------- | -- | -------------- | -----
Root | - | 00 | undefined | first two bytes of the payload define the amount of immediate children. see above.
Byte | Byte | 01 | 1 | -
SByte | SByte | 02 | 1 | -
Int16 | Int16 | 03 | 2 | -
UInt16 | UInt16 | 04 | 2 | -
Int32 | Int32 | 05 | 4 | -
UInt32 | UInt32 | 06 | 4 | -
Int64 | Int64 | 07 | 8 | -
UInt64 | UInt64 | 08 | 8 | -
Half | Half | 09 | 2 | -
Single | Single | 0A | 4 | -
Double | Double | 0B | 8 | -
String | Byte[] | 0C | undefined | 2 bytes defining the length of the string, followed by the string in UTF-8. in libraries, this should be represented as `System.String` regardless.
String16 | String | 0D | undefined | 2 bytes defining the amount of UTF-16 characters in the string, followed by the string in UTF-16. The total binary length of the string is therefore twice the given length.
DateTime | DateTimeOffset | 0E | 10 | first 8 bytes define the `DateTimeOffset.Ticks` property, latter 2 bytes store the offset from UTC in minutes.
Date | DateOnly | 0F | 4 | the 4 bytes define the `DateOnly.DayNumber` property.
Time | TimeOnly | 10 | 8 | the 8 bytes define the `TimeOnly.Ticks` property.
ByteArray | Byte[] | 11 | undefined | 2 bytes defining the length of the array, followed by the array
SByteArray | SByte[] | 12 | undefined | ^
Int16Array | Int16[] | 13 | undefined | ^
UInt16Array | UInt16[] | 14 | undefined | ^
Int32Array | Int32[] | 15 | undefined | ^
UInt32Array | UInt32[] | 16 | undefined | ^
Int64Array | Int64[] | 17 | undefined | ^
UInt64Array | UInt64[] | 18 | undefined | ^
HalfArray | Half[] | 19 | undefined | ^
SingleArray | Single[] | 1A | undefined | ^
DoubleArray | Double[] | 1B | undefined | ^
Guid | Guid | 1C | 16 | -
List | List<T> | 1D | undefined | 2 bytes defining the length of the list, followed by another byte defining the type of all children, followed by the list payload. This type cannot be `List`.
Compound | - | 1E | undefined | 2 bytes defining the amount of immediate children, followed by the children.

---

Booleans should be represented in C style as `Byte`s

### Examples

A valid Castle file would, for instance, be:

~~~
00 07 00 00 00 00 00 05 00 48 65 6C 6C 6F 01 00 0C 07 00 00 00 00 00 57 6F 72 6C 64
~~~

Split up, `00` declares this to be a root token.

This is followed by `07 00 00 00` defining the total length of the name array (in little endian) to be 7 bytes and another `00 00`. This would be interpreted as name array index, but since this is the root token, it remains unnamed.

The following `05 00` defines the length of the first name, again in little endian, to be 5 bytes. This name is `48 65 6C 6C 6F`, the ASCII representation for `Hello`.

This is in turn followed by `01 00`, defining this root token has exactly one child.

This child has a total length of `07 00 00 00`, 7 bytes and is of type `0C`, String.

`00 00` is passed as name deduplication ID and read as `Names[0]`, returning the first element of the name array.

Finally, this string token payload is `57 6F 72 6C 64`, `World`.

Since a string cannot have children, and we have already established that the root has one child, the file ends here. All further data should be ignored by implementing parsers.

---

Another valid Castle file would be:

~~~
0C 00 00 00 00 00 06 00 6E 75 6D 62 65 72 02 00 48 69 02 00 0A 00 00 0F 0F 0F 0F 0A 00 00 F0 F0 F0 F0
~~~

This contains two float tokens, one with the name `number` and one with the name `Hi`. The first one stores `0F 0F 0F 0F`, around 7.0533; the second one stores `F0 F0 F0 F0`, -596541423374289729685825781760.

## Conversion to and from the NBT format

All valid NBT data can be seamlessly converted into Castle data. The other way around it is less simple.

As a first step, a converter must convert all primitives into NBT-compliant primitives. In case of unsigned to signed integers; a converter should choose to increase the word length instead of capping the limit where possible. If the output data requires a specific type or it is a `UInt64` token, it should instead hard limit the value. All `Half` tokens should be converted to `Single`.

As `System.Text.Encoding.UTF8` handles converting strings for us, all we have to do is store both string tokens in a type the NBT serializer can understand.

Secondarily, the array tokens should be converted to list tokens, again adhering by the same rules as specified above for primitive types. Exempt from this are `SByteArray`, `Int32Array` and `Int64Array` tokens, as the NBT specification supports these.

Third, `Guid` tokens should be dissolved in either two `Int64` or four `Int32` tokens, depending on the target data requirements.

Fourth, `DateTime`, `Date` and `Time` tokens should be converted in the way the target data requires, usually as integers. NBT has no counterpart for this and is hardly used to store absolute times in this way.

Fifth, all names need to be re-duplicated from the root token into their respective tokens.

Sixth, the root token needs to be dissolved and replaced by a compound.
