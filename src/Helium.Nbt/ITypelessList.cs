namespace Helium.Nbt;

internal interface ITypelessList // solely exists for pattern matching
{
	public NbtTokenType ListTokenType { get; set; }
}
