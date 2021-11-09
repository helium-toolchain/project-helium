namespace Helium.Nbt;

using System;

/// <summary>
/// Lists every single NBT token type by its binary ID
/// </summary>
public enum NbtTokenType : Byte
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
