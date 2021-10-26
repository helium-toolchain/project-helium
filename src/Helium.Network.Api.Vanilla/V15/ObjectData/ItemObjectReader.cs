﻿namespace Helium.Network.Api.Vanilla.V15.ObjectData;

using System;

/// <summary>
/// Extensions to convert C0x00.Data fields for items (C0x00.Type 2)
/// </summary>
public static class ItemObjectReader
{
	/// <summary>
	/// Reads from packet data.
	/// </summary>
	public static Boolean IsItemVelocityActive(this Int32 objectData)
	{
		return objectData == 1;
	}

	/// <summary>
	/// Writes to packet data.
	/// </summary>
	public static Int32 SetItemVelocityActive(this Boolean itemVelocityActive)
	{
		return itemVelocityActive ? 1 : 0;
	}
}