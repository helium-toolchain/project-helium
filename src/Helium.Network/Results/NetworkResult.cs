namespace Helium.Network.Results;

using System;

/// <summary>
/// Represents a no-return result of a Helium networking operation.
/// </summary>
public readonly struct NetworkResult : INetworkResult
{
	/// <inheritdoc/>
	public OperationOutcome Outcome { get; init; }

	/// <inheritdoc/>
	public Exception? Exception { get; init; }
}
