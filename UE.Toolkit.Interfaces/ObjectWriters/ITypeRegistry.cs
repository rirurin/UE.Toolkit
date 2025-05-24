using System.Diagnostics.CodeAnalysis;

namespace UE.Toolkit.Interfaces.ObjectWriters;

/// <summary>
/// Type registry interface.
/// </summary>
public interface ITypeRegistry
{
    /// <summary>
    /// Register a type provider for object XMLs.
    /// </summary>
    /// <param name="provider">Type provider instance.</param>
    void RegisterProvider(ITypeProvider provider);
    
    /// <summary>
    /// Try and get a type with the given name, optional hint, or with specific provider.
    /// </summary>
    /// <param name="typeName">Type name.</param>
    /// <param name="typeHint">Type hint to prefixed Unreal type.</param>
    /// <param name="providerId">Type provider to get type from.</param>
    /// <param name="type">The requested type, if found.</param>
    /// <returns>Whether the requested type was found.</returns>
    bool TryGetType(string typeName, string? typeHint, string? providerId, [NotNullWhen(true)] out Type? type);
    
    /// <summary>
    /// Try and get a type with the given name, optional hint, or with specific provider.
    /// </summary>
    /// <param name="typeName">Type name.</param>
    /// <param name="providerId">Type provider to get type from.</param>
    /// <param name="type">The requested type, if found.</param>
    /// <returns>Whether the requested type was found.</returns>
    bool TryGetType(string typeName, string? providerId, [NotNullWhen(true)] out Type? type);
}