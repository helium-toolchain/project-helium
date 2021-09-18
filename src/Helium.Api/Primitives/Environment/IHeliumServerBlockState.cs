namespace Helium.Api.Primitives.Environment;

using System;

/// <summary>
/// Base interface for Helium Server Block States. Contains all networking crucial information.
/// </summary>
public interface IHeliumServerBlockState
{
	/// <summary>
	/// Blockstate Key. This should always be assigned by a registry system, never hardcoded.
	/// </summary>
	public UInt32 Key { get; }

	/// <summary>
	/// Blockstate value. This is passed down to the client/core, respectively, for verification. 
	/// The Helium Server itself does not check the type.
	/// </summary>
	public Object Value { get; set; }

	/// <summary>
	/// Blockstate default value. This is passed down to the client/core, respectively, for verification. 
	/// The Helium Server itself does not check the type.
	/// </summary>
	public Object DefaultValue { get; }
}
