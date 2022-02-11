namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleDoubleToken : ICastleToken, IValueToken<Double>
{
	public static Byte Declarator => 0x0B;

	public UInt16 NameId { get; internal set; }

	public Double Value { get; set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleDoubleToken {NameId}:{Value} was not of type CastleRootToken");

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
		return new NbtDoubleToken
		{
			Name = this.Name,
			Value = this.Value
		};
	}
}
