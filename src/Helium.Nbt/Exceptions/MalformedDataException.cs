namespace Helium.Nbt.Exceptions;

using System;

/// <summary>
/// Indicates the data pending NBT conversion was malformed.
/// </summary>
public class MalformedDataException : Exception
{
	public MalformedDataException(String message) : base(message) { }
}
