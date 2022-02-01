# IArrayToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface IArrayToken<T> : IDataToken, IList<T>
~~~

An empty interface to define array sub-tokens.

## See also

- [IDataToken](./datatoken.md)