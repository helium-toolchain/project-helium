namespace Helium.Scripting.Intermediate;

using System;

/// <summary>
/// Constitutes opcodes for the Delta Scripting Engine.
/// </summary>
public enum DeltaVMInstruction : UInt16
{
	/// <summary>
	/// A no-op, used to pad Delta-IR streams.
	/// </summary>
	Nop = 0x0000,

	/// <summary>
	/// Calls an existing command by name.
	/// </summary>
	Call = 0x0001,

	/// <summary>
	/// Performs an integrated call.
	/// </summary>
	ICall = 0x0002,

	/// <summary>
	/// Calls a function loaded into the VM.
	/// </summary>
	FCall = 0x0003,

	/// <summary>
	/// Calls an existing command by name as a tail call.
	/// </summary>
	/// <remarks>
	/// This instruction must immediately precede <see cref="Ret"/>.
	/// </remarks>
	TailCall = 0x0101,

	/// <summary>
	/// Performs an integrated call as a tail call.
	/// </summary>
	/// <remarks>
	/// This instruction must immediately precede <see cref="Ret"/>.
	/// </remarks>
	TailICall = 0x0102,

	/// <summary>
	/// Calls a function loaded into the VM as a tail call.
	/// </summary>
	/// <remarks>
	/// This instruction must immediately precede <see cref="Ret"/>.
	/// </remarks>
	TailFCall = 0x0103,

	/// <summary>
	/// Clears the argument stack.
	/// </summary>
	ASClear = 0x0004,

	/// <summary>
	/// Pushes the top of the execution stack to the argument stack.
	/// </summary>
	ASPush = 0x0005,

	/// <summary>
	/// Clears the execution stack.
	/// </summary>
	ESClear = 0x0006,

	/// <summary>
	/// Pushes a constant to the execution stack.
	/// </summary>
	ESPush = 0x0007,

	/// <summary>
	/// Pops the current top of the execution stack
	/// </summary>
	ESPop = 0x0008,

	/// <summary>
	/// Returns the current VM stack frame, clearing both stacks.
	/// </summary>
	Ret = 0xFFFF
}
