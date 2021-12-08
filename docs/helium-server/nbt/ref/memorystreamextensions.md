# MemoryStreamExtensions

~~~cs
namespace Helium.Nbt;

public static class MemoryStreamExtensions
~~~

Provides some extensions to `MemoryStream` and `Byte`, used in `Helium.Nbt.StringifiedNbtReader`.

## Methods

~~~cs
public static void Skip(this MemoryStream stream, Int32 count)
~~~

Advances the stream by the given number of bytes without reading.

---

~~~cs
public static Byte Peek(this MemoryStream stream)
~~~

Returns the next byte in the stream without advancing the stream.

---

~~~cs
public static Byte Peek(this MemoryStream stream, Int32 count)
~~~

Returns the byte at the given offset from the current position without advancing the stream.

---

~~~cs
public static Boolean Expect(this MemoryStream stream, Byte value)
~~~

Returns whether the next non-whitespace character in the stream matches the given byte.

---

~~~cs
public static void SkipWhitespace(this MemoryStream stream)
~~~

Skips all leading whitespaces in the stream.

---

~~~cs
public static Boolean IsLetter(this Byte value)
~~~

Returns whether the given byte is an alphabetical letter.

---

~~~cs
public static Boolean IsNumber(this Byte value)
~~~

Returns whether the given byte is a numerical letter.

---

~~~cs
public static Boolean IsAlphanumeric(this Byte value)
~~~

Returns whether the given byte is an alphanumeric character.

## See also

- [`StringifiedNbtReader`](./stringifiednbtreader.md)