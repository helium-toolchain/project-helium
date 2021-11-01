namespace Helium.Nbt.Exceptions;

using System;

/// <summary>
/// Indicates the NBT stream was malformed.
/// </summary>
public class MalformedNbtException : Exception
{
	public MalformedNbtException(String message) : base(message) { }
}
