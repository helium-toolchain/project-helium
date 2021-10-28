namespace Helium.Network.Api.Vanilla.V15;

using System;

internal static class LengthExtensions
{
	public static Int32 CalculateLength(this Statistic[] stats)
	{
		Int32 length = 0;

		foreach(Statistic s in stats)
		{
			length += s.Length;
		}

		return length;
	}
}

