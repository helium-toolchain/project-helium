# IValuedComplexNbtToken

~~~cs
namespace Helium.Nbt;

[RequiresPreviewFeatures]
public interface IValuedComplexNbtToken<TValueType> : IComplexNbtToken
~~~

This interface serves as base interface for all complex NBT tokens with a specified enumeration type (usually `INbtToken`).

## Methods

~~~cs
public void AddChild(TValueType token)
~~~

Adds a child token to the current `IValuedComplexNbtToken` implementation.