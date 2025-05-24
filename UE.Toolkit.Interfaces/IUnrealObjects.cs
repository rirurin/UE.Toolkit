using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal;
using UE.Toolkit.Core.Types.Wrappers;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Interfaces;

/// <summary>
/// API for functionality related to Unreal objects.
/// </summary>
public unsafe interface IUnrealObjects : IObjectCreator
{
    /// <summary>
    /// Listen for an object's creation of the given name.
    /// </summary>
    /// <param name="objName">Object name.</param>
    /// <param name="callback">Callback given each object instance.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <remarks>Implemented as a hook on <c>UObject::PostLoadSubobjects</c>, allowing for editing object data before use.</remarks>
    void OnObjectLoadedByName<TObject>(string objName, Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged;

    /// <summary>
    /// Listen for an object's creation of the given name.
    /// </summary>
    /// <param name="callback">Callback given each object instance.</param>
    /// <typeparam name="TObject">Object name and type.</typeparam>
    /// <remarks>Implemented as a hook on <c>UObject::PostLoadSubobjects</c>, allowing for editing object data before use.</remarks>
    void OnObjectLoadedByName<TObject>(Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged;
    
    /// <summary>
    /// Listen for an object's creation of the given class.
    /// </summary>
    /// <param name="callback">Callback given each object instance.</param>
    /// <typeparam name="TObject">Object class type.</typeparam>
    /// <remarks>Implemented as a post-hook on <c>UObject::PostLoadSubobjects</c>, allowing for editing object data before use.</remarks>
    void OnObjectLoadedByClass<TObject>(Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged;
    
    /// <summary>
    /// Listen for an object's creation of the given class.<br/>
    /// Class name should include the expected type prefix: UObjects = U,  AActors = A, Structs = F
    /// </summary>
    /// <param name="objClass">Class name.</param>
    /// <param name="callback">Callback given each object instance.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <remarks>Implemented as a post-hook on <c>UObject::PostLoadSubobjects</c>, allowing for editing object data before use.</remarks>
    void OnObjectLoadedByClass<TObject>(string objClass, Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged;

    /// <summary>
    /// Gets the global UObject array.
    /// </summary>
    FUObjectArray* GUObjectArray { get; }

    /// <summary>
    /// Get the string value of an <see cref="FText"/>.
    /// </summary>
    string FTextToString(FText* text);
}