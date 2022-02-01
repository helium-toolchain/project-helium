# IListToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface IListToken : IDataToken, IList<DataToken>
~~~

## Properties

---

~~~cs
public Byte ListTypeDeclarator { get; }
~~~

Declares the type for all tokens within this list.

## Methods

---

~~~cs
public void AddChildToken(IDataToken token)
~~~

Adds a child token of the same type as the existing list to the list.

## See also

- [IDataToken](./idatatoken)