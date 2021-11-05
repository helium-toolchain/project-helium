namespace Helium.Nbt.Internal;

using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct Int64BigEndian
{
	[FieldOffset(0)]
	public Int64 Value;

	[FieldOffset(0)]
	public Memory<Byte> AsByte;

	public Int64 AsSystemDefault()
	{
		return BinaryPrimitives.ReadInt64BigEndian(AsByte.Span);
	}

	public void FromSystemDefault(Int64 value)
	{
		Span<Byte> m = stackalloc Byte[8];
		BinaryPrimitives.WriteInt64BigEndian(m, value);

		this.Value = MemoryMarshal.Cast<Byte, Int64>(m)[0];
	}
}
