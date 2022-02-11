namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;
using Helium.Data.Nbt;

[RequiresPreviewFeatures]
public record struct CastleStringToken : ICastleToken, IValueToken<String>
{
	private Byte[] tokenValue;

	public static Byte Declarator => 0x0C;

	public UInt16 NameId { get; internal set; }

	public String Value
	{
		get => Encoding.UTF8.GetString(tokenValue);
		set => tokenValue = Encoding.UTF8.GetBytes(value);
	}

	public Byte RefDeclarator => Declarator;

	public String Name
	{
		get => (this.RootToken as CastleRootToken)?.TokenNames[NameId]!;
		set
		{
			CastleRootToken root = this.RootToken as CastleRootToken ?? throw new ArgumentException(
				$"Root token of CastleStringToken {NameId}:{Value} was not of type CastleRootToken");

			if(!root.TokenNames.Contains(value))
			{
				root.TokenNames.Add(value);
			}

			this.NameId = (UInt16)root.TokenNames.IndexOf(value);
		}
	}

	public IRootToken? RootToken { get; set; }

	public IDataToken? ParentToken { get; set; }

	public IDataToken ToNbtToken()
	{
		return new NbtStringToken
		{
			Name = this.Name,
			Value = this.Value
		};
	}
}
