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
    /// This method works any type derived from UObject, which will be any type prefixed with U or A.
    /// </summary>
    /// <param name="Value">Type information for the specified object type.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <returns>If the type information exists.</returns>
    public bool GetClassInfoFromClass<TObject>(out IUClass? Value) where TObject : unmanaged;
    
    /// <summary>
    /// Get the type information for a specified object. If this class has no type info, this will return null.
    /// This method works any type derived from UObject, which will be any type prefixed with U or A.
    /// </summary>
    /// <param name="Name">Name of the object type.</param>
    /// <param name="Value">Type information for the object type with the given name.</param>
    /// <returns>If the type information exists.</returns>
    public bool GetClassInfoFromName(string Name, out IUClass? Value);
    
    /// <summary>
    /// Get the type information for a specified object. If this struct has no type info, this will return null.
    /// This method works on types that are prefixed with F.
    /// </summary>
    /// <param name="Value">Type information for the specified object type.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <returns>If the type information exists.</returns>
    public bool GetScriptStructInfoFromType<TObject>(out IUScriptStruct? Value) where TObject : unmanaged;
    
    /// <summary>
    /// Get the type information for a specified object. If this struct has no type info, this will return null.
    /// This method works on types that are prefixed with F.
    /// </summary>
    /// <param name="Name">Name of the object type.</param>
    /// <param name="Value">Type information for the object type with the given name.</param>
    /// <returns>If the type information exists.</returns>
    public bool GetScriptStructInfoFromName(string Name, out IUScriptStruct? Value);

    /// <summary>
    /// Add an Int8 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddI8Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int16 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddI16Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int32 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddI32Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Int64 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddI64Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt8 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddU8Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt16 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddU16Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt32 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddU32Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a UInt64 property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddU64Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a float property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddF32Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a double property to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddF64Property<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;

    /// <summary>
    /// Add a C-style boolean to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddCBoolProperty<TObject>(string Name, int Offset, out IFBoolProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a bitflag-style boolean to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Byte offset of the new field.</param>
    /// <param name="Bit">Bit offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddBitBoolProperty<TObject>(string Name, int Offset, int Bit, out IFBoolProperty? Out) where TObject : unmanaged;

    /// <summary>
    /// Add a by-value struct to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <typeparam name="TField">Field type.</typeparam>
    public bool AddStructProperty<TObject, TField>(string Name, int Offset, out IFStructProperty? Out)
        where TObject : unmanaged
        where TField : unmanaged;
    
    /// <summary>
    /// Add a by-reference class in the form of a raw pointer to the object's class with the specified name and offset.
    /// This will make the field exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <typeparam name="TField">Field type.</typeparam>
    public bool AddObjectProperty<TObject, TField>(string Name, int Offset, out IFObjectProperty? Out)
        where TObject : unmanaged
        where TField : unmanaged;
    
    /// <summary>
    /// Add a by-reference class in the form of a type-safe pointer to the object's class with the specified name and offset.
    /// This will make the field exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <typeparam name="TField">Field type.</typeparam>
    public bool AddClassProperty<TObject, TField>(string Name, int Offset, out IFClassProperty? Out)
        where TObject : unmanaged
        where TField : unmanaged;
    
    /// <summary>
    /// Add a FName to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddNameProperty<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a FString to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddStringProperty<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a FText to the object's class with the specified name and offset. This will make the field
    /// exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Out">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddTextProperty<TObject>(string Name, int Offset, out IFProperty? Out) where TObject : unmanaged;
    
    /// <summary>
    /// Add a Array (TArray<T>) containing elememts of the property defined in Inner to the object's class with the
    /// specified name and offset. This will make the field exposable to blueprints and Object XML.
    /// </summary>
    /// <param name="Name">Name of the new field.</param>
    /// <param name="Offset">Offset of the new field.</param>
    /// <param name="Inner">The inner property used for each entry in the array.</param>
    /// <param name="Property">Return value.</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    public bool AddArrayProperty<TObject>(string Name, int Offset, IFProperty Inner, out IFArrayProperty? Property) 
        where TObject : unmanaged;
}