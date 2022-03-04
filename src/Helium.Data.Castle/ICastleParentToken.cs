namespace Helium.Data.Castle;

using System;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface ICastleParentToken : ICastleToken, IDataToken
{
	public void AddChildToken(IDataToken child);

	public UInt16 DeclaredLength { get; }
}
