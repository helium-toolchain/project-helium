namespace Helium.Data.Nbt.Serialization;

using System;

internal record struct CurrentTokenState
{
    public CurrentTokenType CurrentTokenType { get; init; }
    public Int32 RemainingChildren { get; set; }
    public NbtTokenType ListTokenType { get; set; }
}
