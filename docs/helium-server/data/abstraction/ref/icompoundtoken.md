# ICompoundToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface ICompoundToken : IDataToken, IDictionary<String, IDataToken>
~~~

An interface defining a basic compound token layout.

## Properties

---

~~~cs
public IEnumerable<IDataToken> Children { get; }
~~~

A list of child token for this compound.

## Methods

---

~~~cs
public void AddChildToken(IDataToken)
~~~

Adds a child token to this compound.

## See also

- [IDataToken](./idatatoken)
- [IRootToken](./iroottoken)