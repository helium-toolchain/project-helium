namespace Helium.Api.Mojang;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Position as specified by Mojang's protocol - 26 bit X, 12 bit Y, 26 bit Z
/// <para>
///		Limits: x and z between -33554432 and 33554431, y between -2048 and 2047.
/// </para>
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct Position
{
	/// <summary>
	/// X-coordinate of this position, 26-bit integer.
	/// </summary>
	[FieldOffset(0)]
	public Int32 X;

	/// <summary>
	/// Y-coordinate of this position, 12-bit integer.
	/// </summary>
	[FieldOffset(26)]
	public Int16 Y;

	/// <summary>
	/// Z-coordinate of this position, 26-bit integer.
	/// </summary>
	[FieldOffset(38)]
	public Int32 Z;
}
