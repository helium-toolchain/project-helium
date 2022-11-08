namespace Helium.Network.Results;

using System;

/// <summary>
/// Represents a strongly-typed, returning result.
/// </summary>
/// <typeparam name="TReturn">The return type underlying the operation.</typeparam>
public readonly struct NetworkResult<TReturn> : INetworkResult
{
	/// <inheritdoc/>
	public OperationOutcome Outcome { get; init; }

	/// <inheritdoc/>
	public Exception? Exception { get; init; }

	/// <summary>
	/// The return value represented by this result.
	/// </summary>
	public TReturn? ReturnValue { get; init; }
}
