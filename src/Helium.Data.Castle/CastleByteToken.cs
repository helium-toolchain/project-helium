namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

/// <summary>
/// Represents a Castle byte token.
/// </summary>
[RequiresPreviewFeatures]
public record struct CastleByteToken : ICastleToken, IValueToken<Byte>
{
	public static Byte Declarator => 0x01;

	public UInt16 NameId { get; internal set; }

	public Byte Value { get; set; }

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleByteToken {NameId}:{Value} was not of type CastleRootToken");

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
		return new NbtSByteToken
		{
			Name = this.Name,
			Value = (SByte)this.Value
		};
	}
}
