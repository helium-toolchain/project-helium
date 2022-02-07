namespace Helium.Data.Abstraction;

using System.Collections.Generic;
using System.Runtime.Versioning;

/// <summary>
/// Empty interface to define array tokens.
/// </summary>
/// <typeparam name="T">The type this array stores and serializes.</typeparam>
[RequiresPreviewFeatures]
public interface IArrayToken<T> : IDataToken, IList<T>
{
}
