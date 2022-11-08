namespace Helium.Network.Results;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a no-return result of a Helium networking operation.
/// </summary>
public readonly struct NetworkResult : INetworkResult
{
	/// <inheritdoc/>
	public OperationOutcome Outcome { get; init; }

	/// <inheritdoc/>
	public Exception? Exception { get; init; }

	/// <inheritdoc/>
	public Boolean IsSuccess
	{
		get => this.Outcome is OperationOutcome.Success or OperationOutcome.NoOperation;
	}

	/// <summary>
	/// Returns a new successful result.
	/// </summary>
	public static NetworkResult FromSuccess()
	{
		return new()
		{
			Exception = null,
			Outcome = OperationOutcome.Success,
		};
	}

	[StackTraceHidden]
	public void Throw()
	{
		if(this.Exception is not null)
		{
			throw this.Exception;
		}
	}
}
