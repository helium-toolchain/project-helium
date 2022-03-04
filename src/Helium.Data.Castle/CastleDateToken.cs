namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public record struct CastleDateToken : ICastleToken, IValueToken<DateOnly>
{
	public static Byte Declarator => 0x0F;

	public UInt16 NameId { get; internal set; }

	public DateOnly Value { get; set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleDateToken {NameId}:{Value} was not of type CastleRootToken");

			if(!root.TokenNames.Contains(value))
			{
				root.TokenNames.Add(value);
			}

			this.NameId = (UInt16)root.TokenNames.IndexOf(value);
		}
	}

	public IRootToken? RootToken { get; init; }

	public IDataToken? ParentToken { get; set; }

	public IDataToken ToNbtToken()
	{
		throw new NotImplementedException("NBT does not have a counterpart of this token type.");
	}
}
