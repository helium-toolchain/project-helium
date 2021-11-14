namespace Helium.Nbt;

using System;

internal interface ITypelessList // solely exists for pattern matching
{
	public NbtTokenType ListTokenType { get; set; }
	public Int32 TargetLength { get; set; }
}
