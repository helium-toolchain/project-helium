# IValueToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface IValueToken<T> : IDataToken
~~~

Represents a data token holding a singular primitive.

## Properties

---

~~~cs
public T Value { get; }
~~~

The primitive value of this token.

## See also

- [IDataToken](./idatatoken)