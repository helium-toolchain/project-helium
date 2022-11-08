namespace Helium.Network.Api.Vanilla;

using System;
using System.Runtime.CompilerServices;

using Helium.Network.Results;

/// <summary>
/// Represents a handler for parsing vanilla packets.
/// </summary>
public ref struct VanillaPacketHandler
{
	public ReadOnlySpan<Byte> Payload { get; private set; }

	public Int32 Position { get; private set; }

	public VanillaPacketHandler
	(
		ReadOnlySpan<Byte> payload
	)
	{
		this.Payload = payload;
		this.Position = 0;
	}

	#region unsafe reading primitives
	public SByte ReadSByteUnsafe()
	{
		return Unsafe.As<Byte, SByte>
		(
			ref Unsafe.AsRef
			(
				this.Payload[this.Position++]
			)
		);
	}
	#endregion

	#region reading primitives
	public NetworkResult<SByte> ReadSByte()
	{
		if(this.Position == this.Payload.Length)
		{
			return new()
			{
				Exception = new IndexOutOfRangeException
				(
					$"The current position was too far advanced to read a value of type {typeof(SByte)}"
				),
				ReturnValue = default,
				Outcome = OperationOutcome.Failure
			};
		}
		else
		{
			return NetworkResult<SByte>.FromSuccess
			(
				this.ReadSByteUnsafe()
			);
		}
	}
	#endregion
}
