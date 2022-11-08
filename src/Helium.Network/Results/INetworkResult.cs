namespace Helium.Network.Results;

using System;

/// <summary>
/// Represents a high-performance result type for Helium networking.
/// </summary>
public interface INetworkResult
{
	/// <summary>
	/// The outcome of this operation.
	/// </summary>
	public OperationOutcome Outcome { get; }

	/// <summary>
	/// An exception describing the failure, if applicable and possible.
	/// </summary>
	public Exception? Exception { get; }

	/// <summary>
	/// Indicates whether this result was successful.
	/// </summary>
	public Boolean IsSuccess { get; }

	/// <summary>
	/// Throws the exception stored in this result, if applicable.
	/// </summary>
	public void Throw();
}
