using Reloaded.Hooks.ReloadedII.Interfaces;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

namespace UE.Toolkit.Core.Types.Unreal.Factories;

public abstract class BaseUnrealFactory : IUnrealFactory
{
    public IUnrealMemoryInternal Allocator { get; }
    public IReloadedHooks Hooks { get; }

    public T Cast<T>(IPtr obj)
    {
        var typeName = typeof(T).Name;
        switch (typeName)
        {
            case nameof(IUObject):
                return (T)CreateUObject(obj.Ptr);
            case nameof(IUClass):
                return (T)CreateUClass(obj.Ptr);
            case nameof(IUScriptStruct):
                return (T)CreateUScriptStruct(obj.Ptr);
            case nameof(IUEnum):
                return (T)CreateUEnum(obj.Ptr);
            case nameof(IUUserDefinedEnum):
                return (T)CreateUUserDefinedEnum(obj.Ptr);
            case nameof(IFByteProperty):
                return (T)CreateFByteProperty(obj.Ptr);
            case nameof(IFEnumProperty):
                return (T)CreateFEnumProperty(obj.Ptr);
            case nameof(IFObjectProperty):
                return (T)CreateFObjectProperty(obj.Ptr);
            case nameof(IFSoftClassProperty):
                return (T)CreateFSoftClassProperty(obj.Ptr);
            case nameof(IFClassProperty):
                return (T)CreateFClassProperty(obj.Ptr);
            case nameof(IFStructProperty):
                return (T)CreateFStructProperty(obj.Ptr);
            case nameof(IFMapProperty):
                return (T)CreateFMapProperty(obj.Ptr);
            case nameof(IFInterfaceProperty):
                return (T)CreateFInterfaceProperty(obj.Ptr);
            case nameof(IFArrayProperty):
                return (T)CreateFArrayProperty(obj.Ptr);
            case nameof(IFSetProperty):
                return (T)CreateFSetProperty(obj.Ptr);
            case nameof(IFOptionalProperty):
                return (T)CreateFOptionalProperty(obj.Ptr);
            default:
                throw new NotSupportedException(typeName);
        }
    }

    public abstract IFProperty CreateFProperty(nint ptr);
    public abstract IFByteProperty CreateFByteProperty(nint ptr);
    public abstract IFEnumProperty CreateFEnumProperty(nint ptr);
    public abstract IFObjectProperty CreateFObjectProperty(nint ptr);
    public abstract IFSoftClassProperty CreateFSoftClassProperty(nint ptr);
    public abstract IFClassProperty CreateFClassProperty(nint ptr);
    public abstract IFStructProperty CreateFStructProperty(nint ptr);
    public abstract IFMapProperty CreateFMapProperty(nint ptr);
    public abstract IFInterfaceProperty CreateFInterfaceProperty(nint ptr);
    public abstract IFArrayProperty CreateFArrayProperty(nint ptr);
    public abstract IFSetProperty CreateFSetProperty(nint ptr);
    public abstract IFOptionalProperty CreateFOptionalProperty(nint ptr);
    public abstract IUObjectArray CreateUObjectArray(nint ptr);
    public abstract IUObject CreateUObject(nint ptr);
    public abstract IUClass CreateUClass(nint ptr);
    public abstract IUScriptStruct CreateUScriptStruct(nint ptr);
    public abstract IUEnum CreateUEnum(nint ptr);
    public abstract IUField CreateUField(nint ptr);
    public abstract IUStruct CreateUStruct(nint ptr);
    public abstract IUUserDefinedEnum CreateUUserDefinedEnum(nint ptr);
    public abstract IFFieldClass CreateFFieldClass(nint ptr);
    public abstract IFField CreateFField(nint ptr);

    public IUnrealMemoryInternal GetAllocator() => Allocator;
    public IReloadedHooks GetHooks() => Hooks;

    public BaseUnrealFactory(IUnrealMemoryInternal allocator, IReloadedHooks hooks)
    {
        Allocator = allocator;
        Hooks = hooks;
    }
}