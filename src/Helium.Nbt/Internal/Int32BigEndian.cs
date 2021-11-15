namespace Helium.Nbt.Internal;

using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct Int32BigEndian
{
	[FieldOffset(0)]
	public Int32 Value;

	[FieldOffset(0)]
	public Memory<Byte> AsByte;

	public Int32 AsSystemDefault()
	{
		return BinaryPrimitives.ReadInt32BigEndian(AsByte.Span);
	}

	public void FromSystemDefault(Int32 value)
	{
		Span<Byte> m = stackalloc Byte[4];
		BinaryPrimitives.WriteInt32BigEndian(m, value);

		this.Value = MemoryMarshal.Cast<Byte, Int32>(m)[0];
	}
}
