# MalformedNbtException

~~~cs
namespace Helium.Nbt.Exceptions;

public class MalformedNbtException : Exception
~~~

Indicates the NBT to be read is malformed.

## Constructors

~~~cs
public MalformedNbtException(String message) : base(message)
~~~

Creates a new `MalformedNbtException`.