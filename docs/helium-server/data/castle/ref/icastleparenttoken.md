# ICastleParentToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public interface ICastleParentToken : ICastleToken, IDataToken
~~~

Represents any valid castle parent token: root, compound and list.

## Properties

---

~~~cs
public UInt16 DeclaredLength { get; }
~~~

Stores the declared length of this token.

---

## Methods

---

~~~cs
public void AddChildToken(IDataToken child);
~~~

Requires the implementer to safely add a child token.