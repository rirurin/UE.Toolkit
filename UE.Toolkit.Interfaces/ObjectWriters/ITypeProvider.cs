namespace UE.Toolkit.Interfaces.ObjectWriters;

/// <summary>
/// Type provider contract for data writing.
/// </summary>
public interface ITypeProvider
{
    /// <summary>
    /// ID of type provider, should typically be the mod ID.
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// Whether implementor can provide the concrete type of the given name.
    /// </summary>
    /// <param name="typeName">Type name.</param>
    /// <returns>Whether implementor can provide the type.</returns>
    bool CanProvide(string typeName);

    /// <summary>
    /// Gets the <see cref="Type"/> specified by the name.
    /// </summary>
    /// <param name="typeName">Type name.</param>
    /// <returns>The specified type. This will be cached!</returns>
    Type GetType(string typeName);
}