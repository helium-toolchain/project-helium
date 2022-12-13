﻿namespace Helium.Network.Api.Vanilla.V15.ObjectDataExtensions;

using System;

/// <summary>
/// Extensions to convert C0x00.Data fields for Arrows (C0x00.Type 2 and 72)
/// </summary>
public static class ArrowObjectReader
{
	/// <summary>
	/// Reads from packet data.
	/// </summary>
	public static Int32 AsObjectData(this Int32 data)
	{
		return data - 1;
	}

	/// <summary>
	/// Writes to packet data.
	/// </summary>
	public static Int32 ToObjectData(this Int32 data)
	{
		return data + 1;
	}
}
