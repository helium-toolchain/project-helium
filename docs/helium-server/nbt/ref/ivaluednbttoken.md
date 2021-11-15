# IValuedNbtToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public interface IValuedNbtToken<TValue> : INbtToken
~~~

Serves as base interface for all name-value tokens.

## Properties

~~~cs
public TValue Value { get; }
~~~

Gets the currently stored value of this token.