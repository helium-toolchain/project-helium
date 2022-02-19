# IRootToken

~~~cs
namespace Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public interface IRootToken : ICompoundToken
~~~

Represents the utmost root token of any data structure.

## Properties

---

~~~cs
public abstract static String DataFormat { get; }
~~~

Stores this data structure's concrete type as string. This should be `nbt` for NBT/sNBT data and `helium` for Helium data. Other implementations can specify their own identifier.

## See also

- [ICompoundToken](./icompoundtoken.md)