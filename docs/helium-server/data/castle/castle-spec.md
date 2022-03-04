# Specification

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

## Valid token types

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