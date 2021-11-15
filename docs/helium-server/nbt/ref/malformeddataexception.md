# MalformedDataException

~~~cs
namespace Helium.Nbt.Exceptions;

public class MalformedDataException : Exception
~~~

Indicates the data to be written to NBT is malformed.

## Constructors

~~~cs
public MalformedDataException(String message) : base(message)
~~~

Creates a new `MalformedDataException`.