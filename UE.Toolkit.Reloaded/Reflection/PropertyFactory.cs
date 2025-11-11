using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Reflection;

public abstract class BasePropertyFactory(IUnrealFactory factory, IUnrealMemory memory, IUnrealClasses classes)
{
    private Dictionary<string, FName>? PropertyNames = null;
    
    // Lazy load property name cache, we need to do this to ensure that FName::FName() is valid
    private static Dictionary<string, FName> InitializePropertyNames() => new()
        {
            { "ByteProperty", new FName("ByteProperty", EFindName.FNAME_Find) },
            { "IntProperty", new FName("IntProperty", EFindName.FNAME_Find) },
            { "BoolProperty", new FName("BoolProperty", EFindName.FNAME_Find) },
            { "FloatProperty", new FName("FloatProperty", EFindName.FNAME_Find) },
            { "ObjectProperty", new FName("ObjectProperty", EFindName.FNAME_Find) },
            { "NameProperty", new FName("NameProperty", EFindName.FNAME_Find) },
            { "DelegateProperty", new FName("DelegateProperty", EFindName.FNAME_Find) },
            { "DoubleProperty", new FName("DoubleProperty", EFindName.FNAME_Find) },
            { "ArrayProperty", new FName("ArrayProperty", EFindName.FNAME_Find) },
            { "StructProperty", new FName("StructProperty", EFindName.FNAME_Find) },
            { "VectorProperty", new FName("VectorProperty", EFindName.FNAME_Find) },
            { "RotatorProperty", new FName("RotatorProperty", EFindName.FNAME_Find) },
            { "StrProperty", new FName("StrProperty", EFindName.FNAME_Find) },
            { "TextProperty", new FName("TextProperty", EFindName.FNAME_Find) },
            { "InterfaceProperty", new FName("InterfaceProperty", EFindName.FNAME_Find) },
            { "MulticastDelegateProperty", new FName("MulticastDelegateProperty", EFindName.FNAME_Find) },
            { "LazyObjectProperty", new FName("LazyObjectProperty", EFindName.FNAME_Find) },
            { "SoftObjectProperty", new FName("SoftObjectProperty", EFindName.FNAME_Find) },
            { "Int64Property", new FName("Int64Property", EFindName.FNAME_Find) },
            { "Int32Property", new FName("Int32Property", EFindName.FNAME_Find) },
            { "Int16Property", new FName("Int16Property", EFindName.FNAME_Find) },
            { "Int8Property", new FName("Int8Property", EFindName.FNAME_Find) },
            { "UInt64Property", new FName("UInt64Property", EFindName.FNAME_Find) },
            { "UInt32Property", new FName("UInt32Property", EFindName.FNAME_Find) },
            { "UInt16Property", new FName("UInt16Property", EFindName.FNAME_Find) },
            { "MapProperty", new FName("MapProperty", EFindName.FNAME_Find) },
            { "SetProperty", new FName("SetProperty", EFindName.FNAME_Find) },
        };

    private bool GetProperty(string Name, out FieldClassGlobal? FieldClass)
    {
        FieldClass = null;
        PropertyNames ??= InitializePropertyNames();
        return PropertyNames!.TryGetValue(Name, out var SerialName) 
               && Classes.GetFieldClassGlobal(SerialName, out FieldClass);
    }
    
    protected bool TryGetClassAndProperty<TOwner>(string PropertyName, out IUClass? ClassReflection,
        out FieldClassGlobal? PropertyClass) where TOwner: unmanaged
    {
        ClassReflection = null;
        PropertyClass = null;
        return Classes.GetClassInfoFromClass<TOwner>(out ClassReflection)
               && GetProperty(PropertyName, out PropertyClass);
    }
    
    #region INTERNAL INTERFACE
    
    protected abstract void LinkToPropertyList(IFProperty Property, IUClass Reflect);

    protected abstract void SetPropertySuperFields(IFField Field, string Name, IUClass ClassReflection,
        FieldClassGlobal PropertyClass);
    
    protected abstract EPropertyFlags CreatePropertyFlags(PropertyVisibility Visibility, PropertyBuilderFlags InFlags);

    protected abstract void SetCopyPropertyFields<T>(IFProperty Property, int Offset, PropertyVisibility Visibility)
        where T : unmanaged;
    
    #endregion
   
    #region PUBLIC INTERFACE
    
    public abstract bool CreateI8<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateI16<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateI32<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateI64<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateU8<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateU16<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateU32<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateU64<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateF32<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    public abstract bool CreateF64<TOwner>(out IFProperty? NewProperty, string Name, int Offset,
        PropertyVisibility Visibility) where TOwner: unmanaged;
    
    #endregion

    protected readonly IUnrealFactory Factory = factory;
    protected readonly IUnrealMemory Memory = memory;
    protected readonly IUnrealClasses Classes = classes;

    protected const int FIELD_ALIGNMENT = 0x80;
}

[Flags]
public enum PropertyBuilderFlags
{
    NoCtor = 1 << 0, // ZeroConstructor, can just be memset
    Copy = 1 << 1, // If property can be memcpy'd.
    NoDtor = 1 << 2, // No destructor
    Hash = 1 << 3, // Can be hashed
}