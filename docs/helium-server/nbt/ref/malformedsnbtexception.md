# MalformedSNbtException

~~~cs
namespace Helium.Nbt.Exceptions;

public class MalformedSNbtException : Exception
~~~

Indicates the stringified NBT data to be read is malformed.

## Constructors

~~~cs
public MalformedNbtException(String message) : base(message)
~~~

Creates a new `MalformedSNbtException`.