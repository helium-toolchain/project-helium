namespace Helium.Data.Nbt.Serialization;

using System;

/// <summary>
/// Maps the different IDs to token types.
/// </summary>
public enum NbtTokenType : Byte
{
    End,
    SByte,
    Int16,
    Int32,
    Int64,
    Single,
    Double,
    SByteArray,
    String,
    List,
    Compound,
    Int32Array,
    Int64Array
}
