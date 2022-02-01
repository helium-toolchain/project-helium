# IDataToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface IDataToken
~~~

## Properties

---

~~~cs
public abstract static Byte Declarator { get; }
~~~

Declarator for this token type when serialized to binary data.

---

~~~cs
public Byte RefDeclarator { get; }
~~~

Gets the Declarator via instance. This should always be implemented as `public Byte RefDeclarator => Declarator;`.

---

~~~cs
public String Name { get; }
~~~

Stores the name of this token.

---

~~~cs
public IRootToken? RootToken { get; }
~~~

Stores the root token for this token. Implementing this is optional.

---

~~~cs
public IDataToken? ParentToken { get; }
~~~

Stores the immediate parent token for this token. Implementing this is optional.

## See also

- [IRootToken](./iroottoken)