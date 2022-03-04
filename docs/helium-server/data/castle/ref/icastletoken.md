# ICastleToken

~~~cs
namespace Helium.Data.Castle;

[RequiresPreviewFeatures]
public interface ICastleToken : IDataToken
~~~

Base interface for all castle-type tokens.

## Properties

---

~~~cs
public UInt16 NameId { get; }
~~~

Stores the ID of this tokens name in the root array.

---

## Methods

---

~~~cs
public IDataToken ToNbtToken();
~~~

Should return a NBT representation of this token or throw `NotImplementedException`. This is only intended for rudimentary converters.