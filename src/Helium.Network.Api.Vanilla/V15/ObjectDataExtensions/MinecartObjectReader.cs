namespace Helium.Network.Api.Vanilla.V15.ObjectDataExtensions;

using System;

/// <summary>
/// Extensions to convert C0x00.Data fields for Minecarts (C0x00.Type 42)
/// </summary>
public static class MinecartObjectReader
{
	/// <summary>
	/// Reads from packet data.
	/// </summary>
	public static MinecartObjectState AsObjectState(this Int32 data)
	{
		return (MinecartObjectState)data;
	}

	/// <summary>
	/// Writes to packet data
	/// </summary>
	public static Int32 ToObjectState(this MinecartObjectState state)
	{
		return (Int32)state;
	}
}
