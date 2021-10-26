namespace Helium.Api.Primitives.Entity;

using System;

internal interface IHeliumServerEntity
{
	public Int64 Id { get; }
	public String Namespace { get; }
	public String EntityName { get; }
}
