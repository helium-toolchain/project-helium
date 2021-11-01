namespace Helium.Nbt;

using System;

/// <summary>
/// Lists every single NBT tag type by its binary ID
/// </summary>
public enum NbtTagType : Int32
{
	End,
	Byte,
	Short,
	Int,
	Long,
	Float,
	Double,
	ByteArray,
	String,
	List,
	Compound,
	IntArray,
	LongArray
}
