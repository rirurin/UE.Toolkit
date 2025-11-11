using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

namespace UE.Toolkit.Interfaces;

public interface IUnrealClasses : IUnrealClassesInternal
{
    /// <summary>
    /// Listen for the creation of an object's class, then extend it's allocation size and call a custom constructor
    /// after the existing constructor has executed.
    /// </summary>
    /// <param name="ExtraSize">Number of bytes to increase the allocation size of object instances by.</param>
    /// <param name="callback">A custom constructor to call after the original constructor</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <remarks>
    /// The extension is only created for the specific object type. If a struct/class inherits from this class, it
    /// won't receive an extension.
    /// </remarks>
    public void AddExtension<TObject>(uint ExtraSize, Action<ToolkitUObject<TObject>> callback) 
        where TObject: unmanaged;
    
    /// <summary>
    /// Listen for the creation of an object's class, then call a custom constructor after the existing constructor
    /// has executed.
    /// </summary>
    /// <param name="callback">A custom constructor to call after the original constructor</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddConstructor<TObject>(Action<ToolkitUObject<TObject>> callback)
        where TObject : unmanaged => AddExtension(0, callback);
    
    /// <summary>
    /// Get the type information for a specified object. If this class has no type info, this will return null.
    /// </summary>
    /// <param name="Value">Type information for the specified object type.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <returns>If the type information exists.</returns>
    public bool GetClassInfoFromClass<TObject>(out IUClass? Value) where TObject : unmanaged;
    
    /// <summary>
    /// Get the type information for a specified object. If this class has no type info, this will return null.
    /// </summary>
    /// <param name="Name">Name of the object type.</param>
    /// <param name="Value">Type information for the object type with the given name.</param>
    /// <returns>If the type information exists.</returns>
    public bool GetClassInfoFromName(string Name, out IUClass? Value);

    /// <summary>
    /// Add an Int8 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddI8Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int16 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddI16Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int32 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddI32Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int64 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddI64Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt8 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddU8Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt16 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddU16Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt32 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddU32Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt64 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddU64Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a float property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddF32Property<TObject>(string Name, int Offset) where TObject : unmanaged;
    
    /// <summary>
    /// Add a double property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public void AddF64Property<TObject>(string Name, int Offset) where TObject : unmanaged;
}