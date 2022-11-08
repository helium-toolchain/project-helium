namespace Helium.Network.Results;

using System;

[Flags]
public enum OperationOutcome : Int64
{
	Success = 1 << 1,
	Failure = 1 << 2,
	Unknown = 1 << 3,
	NoOperation = 1 << 4
}
