namespace Helium.Network.Api.Vanilla.CommandData;

using System;
using System.Collections.Generic;

using Helium.Api.Commands;

public static class CommandDataConverter
{
	public static CommandNode[] FromHeliumNodes(IHeliumCommandNode[] nodes)
	{
		Dictionary<Int32, Int32> indices = new();
		CommandNode[] mojang = new CommandNode[nodes.Length];

		for(Int32 i = 0; i < nodes.Length; i++)
		{
			indices.Add(nodes[i].GetHashCode(), i);
		}

		for(Int32 i = 0; i < nodes.Length; i++)
		{
			mojang[i] = CommandNode.FromHeliumNode(nodes[i], indices);
		}

		return mojang;
	}
}
