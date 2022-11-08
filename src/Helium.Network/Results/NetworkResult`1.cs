namespace Helium.Network.Results;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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

	/// <inheritdoc/>
	public Boolean IsSuccess
	{
		get => this.Outcome is OperationOutcome.Success or OperationOutcome.NoOperation;
	}

	/// <summary>
	/// Checks whether the return value of this result is defined.
	/// </summary>
	/// <param name="value">If it is defined, the return value, if not, <see langword="null"/>.</param>
	/// <returns>Whether the return value is defined.</returns>
	public Boolean IsDefined
	(
		[NotNullWhen(true)]
		out TReturn? value
	)
	{
		if(this.Outcome is OperationOutcome.Success or OperationOutcome.NoOperation && this.ReturnValue is not null)
		{
			value = this.ReturnValue;
			return true;
		}
		else
		{
			value = default;
			return false;
		}
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
