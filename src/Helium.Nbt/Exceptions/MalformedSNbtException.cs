namespace Helium.Nbt.Exceptions;

using System;

/// <summary>
/// Indicates the sNBT data was malformed
/// </summary>
public class MalformedSNbtException : Exception
{
	public MalformedSNbtException(String message) : base(message) { }
}
