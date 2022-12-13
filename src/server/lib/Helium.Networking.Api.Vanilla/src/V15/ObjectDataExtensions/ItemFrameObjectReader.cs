﻿namespace Helium.Network.Api.Vanilla.V15.ObjectDataExtensions;

using System;

/// <summary>
/// Extensions to convert C0x00.Data fields for Item Frames (C0x00.Type 36)
/// </summary>
public static class ItemFrameObjectReader
{
	/// <summary>
	/// Reads from packet data.
	/// </summary>
	public static ItemFrameObjectState AsObjectState(this Int32 data)
	{
		return (ItemFrameObjectState)data;
	}

	/// <summary>
	/// Writes to packet data.
	/// </summary>
	public static Int32 ToObjectState(this ItemFrameObjectState state)
	{
		return (Int32)state;
	}
}
