using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Common;
using UE.Toolkit.Reloaded.Common.GameConfigs;
using UE.Toolkit.Reloaded.Reflection;

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealClasses : IUnrealClasses
{
    // Used in Engine version 4.27
    private delegate void GetPrivateStaticClassBody_UE4(nint packageName, nint name, UClass** returnClass, 
        nint registerNativeFunc, uint size, uint align, uint flags, ulong castFlags, nint config, nint inClassCtor, 
        nint vtableHelperCtorCaller, nint addRefObjects, nint superFn, nint withinFn, byte isDynamic, nint dynamicFn);
    private SHFunction<GetPrivateStaticClassBody_UE4> _GetPrivateStaticClassBodyUE4;
    
    private void GetPrivateStaticClassBodyUE4(
        nint packageName,
        nint name,
        UClass** returnClass,
        nint registerNativeFunc,
        uint size,
        uint align,
        uint flags,
        ulong castFlags,
        nint config,
        nint inClassCtor,
        nint vtableHelperCtorCaller, // xor eax,eax, ret
        nint addRefObjects, // ret
        nint superFn, // [superType]::StaticClass
        nint withinFn, // usually UObject::StaticClass
        byte isDynamic,
        nint dynamicFn)
    {
        TryExtendClass(name, inClassCtor, ref size);
        _GetPrivateStaticClassBodyUE4!.Hook!.OriginalFunction(packageName, name, returnClass, registerNativeFunc, size, align, 
            flags, castFlags, config, inClassCtor, vtableHelperCtorCaller, addRefObjects, superFn, withinFn, isDynamic, dynamicFn);
        AddToClassList(name, returnClass);
        TryHookStructLink(returnClass);
    }
    
    // NOTE: 5.0 has it's own signature
    
    // Used in Engine versions 5.1+
    private delegate void GetPrivateStaticClassBody_UE5(nint packageName, nint name, UClass** returnClass, 
        nint registerNativeFunc, uint size, uint align, uint flags, ulong castFlags, nint config, nint inClassCtor, 
        nint vtableHelperCtorCaller, nint staticFn, nint superFn, nint withinFn);
    private SHFunction<GetPrivateStaticClassBody_UE5> _GetPrivateStaticClassBodyUE5;
    private void GetPrivateStaticClassBodyUE5(
        nint packageName,
        nint name,
        UClass** returnClass,
        nint registerNativeFunc,
        uint size,
        uint align,
        uint flags,
        ulong castFlags,
        nint config,
        nint inClassCtor,
        nint vtableHelperCtorCaller, // xor eax,eax, ret
        nint staticFn,
        nint superFn, // [superType]::StaticClass
        nint withinFn) // usually UObject::StaticClass
    {
        TryExtendClass(name, inClassCtor, ref size);
        _GetPrivateStaticClassBodyUE5!.Hook!.OriginalFunction(packageName, name, returnClass, registerNativeFunc, size,
            align, flags, castFlags, config, inClassCtor, vtableHelperCtorCaller, staticFn, superFn, withinFn);
        AddToClassList(name, returnClass);
        TryHookStructLink(returnClass);
    }

    private void TryExtendClass(nint pClassName, nint classCtor, ref uint Size)
    {
        var ClassName = Marshal.PtrToStringUni(pClassName);
        if (ClassName == null) return;
        if (ClassName == null || !ClassExtensions.TryGetValue(ClassName, out var Extension)) return;
        var OldSize = Size;
        Size += Extension.ExtraSize;
        if (classCtor != nint.Zero)
        {
            Extension.ConstructorHook = FollowThunkToGetAppropriateHook(classCtor, Extension.Constructor);
        }

        if (OldSize != Size)
        {
            Log.Debug($"Extended {ClassName}: new size ({OldSize} -> {Size})");
        }
    }

    private void AddToClassList(nint name, UClass** returnClass)
    {
        var ClassName = Marshal.PtrToStringUni(name);
        if (ClassName == null) return;
        Classes.GetOrAdd(ClassName, Factory.CreateUClass((nint)(*returnClass)));
    }
    
    
    private IHook<ClassExtension.ExtensionConstructor> FollowThunkToGetAppropriateHook
        (nint addr, ClassExtension.ExtensionConstructor ctorHook)
    {
        // build a new multicast delegate by injecting the native function, followed by custom code
        // this reference will live for program's lifetime so there's no need to store hook in the caller
        IHook<ClassExtension.ExtensionConstructor>? retHook = null;
        ClassExtension.ExtensionConstructor ctorHookReal = x =>
        {
            if (retHook == null)
            {
                Log.Error("retHook is null. Game will crash.");
                return;
            }
            retHook.OriginalFunction(x);
        };
        var Process = System.Diagnostics.Process.GetCurrentProcess();
        var BaseAddress = Process.MainModule!.BaseAddress;
        ctorHookReal += ctorHook;
        var offset = (int)(addr - BaseAddress);
        var transformed = Address.GetAddressMayThunk(offset);
        // Log.Debug($"Transformed address: {addr:X} -> {transformed:X}");
        retHook = Hooks.CreateHook(ctorHookReal, (long)transformed).Activate();
        return retHook;
    }
    
    private void TryHookStructLink(UClass** ppClass)
    {
        if (_StructLink != null) return;
        var LinkAddr = *(nint*)(*(nint*)(*ppClass) + UStructLinkOffset);
        var Process = System.Diagnostics.Process.GetCurrentProcess();
        var Offset = (int)(LinkAddr - Process.MainModule!.BaseAddress);
        _StructLink = Hooks.CreateHook<UStruct_Link>(UStruct_LinkImpl, (long)Address.GetIndirectAddressShort(Offset)).Activate();
    }
    
    private delegate void UStruct_Link(UStruct* pThis, nint Ar, byte bRelinkExistingProperties);
    private IHook<UStruct_Link>? _StructLink = null;
    private uint UStructLinkOffset;
    private Dictionary<string, FName>? ObjectWithLinkTypes;

    private void UStruct_LinkImpl(UStruct* pThis, nint Ar, byte bRelinkExistingProperties)
    {
        ObjectWithLinkTypes ??= new()
        {
            { "Class", new FName("Class", EFindName.FNAME_Find) },
            { "ScriptStruct", new FName("ScriptStruct", EFindName.FNAME_Find) },
            { "Function", new FName("Function", EFindName.FNAME_Find) },
        };
        _StructLink!.OriginalFunction(pThis, Ar, bRelinkExistingProperties);
        var This = Factory.CreateUStruct((nint)pThis);
        var TypeEnum = StructType.None;
        if (This.ClassPrivate.NamePrivate.Equals(ObjectWithLinkTypes["Class"])) TypeEnum |= StructType.Class;
        else if (This.ClassPrivate.NamePrivate.Equals(ObjectWithLinkTypes["ScriptStruct"])) TypeEnum |= StructType.ScriptStruct;
        if (TypeEnum.HasFlag(StructType.ScriptStruct)) // Add to ScriptStruct list
            ScriptStructs.TryAdd(This.NamePrivate.ToString(), Factory.CreateUScriptStruct(This.Ptr));
        if (TypeEnum != StructType.None)
        {
            var Class = Factory.CreateUClass(This.Ptr);
            foreach (var Property in Class.PropertyLink)
            {
                var FieldTypeName = ((FFieldClass*)Property.ClassPrivate.Ptr)->Name;
                FieldTypes.TryAdd(FieldTypeName.ComparisonIndex.Value,
                    new(Property.VTable, Property.ClassPrivate));
                /*
                if (PropertyFactory.CheckPropertyEquality("ClassProperty", FieldTypeName.ComparisonIndex.Value))
                {
                    var Prop = Factory.CreateFClassProperty(Property.Ptr);
                    var Object = Factory.CreateUObject(This.Ptr);
                    Log.Debug($"{Object.NamePrivate}->{Prop.NamePrivate}: (offset 0x{Prop.Offset_Internal:x}, size 0x{Prop.ElementSize:x}): {Prop.PropertyClass.NamePrivate} (size: 0x{Prop.PropertyClass.PropertiesSize:x})");
                }
                */
                /*
                if (PropertyFactory.CheckPropertyEquality("ArrayProperty", FieldTypeName.ComparisonIndex.Value))
                {
                    var Prop = Factory.CreateFArrayProperty(Property.Ptr);
                    var Object = Factory.CreateUObject(This.Ptr);
                    Log.Debug($"{Object.NamePrivate}->{Prop.NamePrivate}: (offset 0x{Prop.Offset_Internal:x}, size 0x{Prop.ElementSize:x}): TArray<{Prop.Inner.ClassPrivate.Name} {Prop.Inner.NamePrivate}>");
                }
                */
            }
        }
    }
    
    #region IUnrealClasses implementation

    public void AddExtension<TObject>(uint ExtraSize, Action<ToolkitUObject<TObject>> callback)
        where TObject : unmanaged 
        => ClassExtensions.GetOrAdd(typeof(TObject).Name[1..], new ClassExtension(ExtraSize, 
            obj => callback(new((TObject*)*(nint*)obj))));

    public bool GetClassInfoFromClass<TObject>(out IUClass? Value) where TObject : unmanaged
        => GetClassInfoFromName(typeof(TObject).Name, out Value);

    public bool GetClassInfoFromName(string Name, out IUClass? Value)
        => Classes.TryGetValue(Name[1..], out Value);
    
    public bool GetScriptStructInfoFromType<TObject>(out IUScriptStruct? Value) where TObject : unmanaged
        => GetScriptStructInfoFromName(typeof(TObject).Name, out Value);

    public bool GetScriptStructInfoFromName(string Name, out IUScriptStruct? Value)
        => ScriptStructs.TryGetValue(Name[1..], out Value);

    public bool AddI8Property<TObject>(string Name, int Offset, out IFProperty? Property)
        where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateI8<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddI16Property<TObject>(string Name, int Offset, out IFProperty? Property) where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateI16<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }
    
    public bool AddI32Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateI32<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddI64Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateI64<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddU8Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateU8<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddU16Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateU16<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddU32Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateU32<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddU64Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateU64<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddF32Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateF32<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddF64Property<TObject>(string Name, int Offset, out IFProperty? Property) 
    where TObject : unmanaged
    {
        Property = null;
        return  PropertyFactory.CreateF64<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddCBoolProperty<TObject>(string Name, int Offset, out IFBoolProperty? Property)
        where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateCBool<TObject>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddBitBoolProperty<TObject>(string Name, int Offset, int Bit,
        out IFBoolProperty? Property) where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateBitBool<TObject>(out Property, Name, Offset, Bit, PropertyVisibility.Public);
    }


    public bool AddStructProperty<TObject, TField>(string Name, int Offset, out IFStructProperty? Property)
        where TObject : unmanaged
        where TField : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateStruct<TObject, TField>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddObjectProperty<TObject, TField>(string Name, int Offset, out IFObjectProperty? Property)
        where TObject : unmanaged
        where TField : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateObject<TObject, TField>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddClassProperty<TClass, TField>(string Name, int Offset, out IFClassProperty? Property)
        where TClass : unmanaged
        where TField : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateClass<TClass, TField>(out Property, Name, Offset, PropertyVisibility.Public);
    }

    public bool AddNameProperty<TObject>(string Name, int Offset, out IFProperty? Property)
        where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateName<TObject>(out Property, Name, Offset, PropertyVisibility.Public);   
    }

    public bool AddStringProperty<TObject>(string String, int Offset, out IFProperty? Property)
        where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateString<TObject>(out Property, String, Offset, PropertyVisibility.Public);
    }

    public bool AddTextProperty<TObject>(string Text, int Offset, out IFProperty? Property)
        where TObject : unmanaged
    {
        Property = null;
        return PropertyFactory.CreateText<TObject>(out Property, Text, Offset, PropertyVisibility.Public);
    }

    public bool AddArrayProperty<TObject>(string Name, int Offset, IFProperty Inner, 
        out IFArrayProperty? Property) where TObject : unmanaged
    {
        Property = null;
        return false;
    }

    #endregion
    
    #region IUnrealClassesInternal implementation

    public bool GetFieldClassGlobal(FName Name, out FieldClassGlobal FieldClass)
        => FieldTypes.TryGetValue(Name.ComparisonIndex.Value, out FieldClass);
    
    #endregion

    private ConcurrentDictionary<string, ClassExtension> ClassExtensions = new();
    private ConcurrentDictionary<string, IUClass> Classes = new();
    private ConcurrentDictionary<string, IUScriptStruct> ScriptStructs = new();
    private ConcurrentDictionary<ulong, FieldClassGlobal> FieldTypes = new();
    
    private IUnrealFactory Factory;
    private IUnrealMemory Memory;
    private IReloadedHooks Hooks;
    private ResolveAddress Address;

    private BasePropertyFactory PropertyFactory;

    public UnrealClasses(IUnrealFactory _Factory, IUnrealMemory _Memory, IReloadedHooks _Hooks, ResolveAddress _Address)
    {
        Factory = _Factory;
        Memory = _Memory;
        Hooks = _Hooks;
        Address = _Address;

        PropertyFactory = GameConfig.Instance.Id switch
        {
            "P3R" => new Reflection.UE4_27_2.PropertyFactory(Factory, Memory, this),
            _ => new Reflection.UE5_4_4.PropertyFactory(Factory, Memory, this),
        };
        
        Project.Inis.UsingSetting<uint>(Constants.UnrealIniId, "Link", nameof(UStruct),
            value => UStructLinkOffset = value);
        
        _GetPrivateStaticClassBodyUE4 = new(GetPrivateStaticClassBodyUE4);
        _GetPrivateStaticClassBodyUE5 = new(GetPrivateStaticClassBodyUE5);
    }
}

public class ClassExtension(uint extraSize, ClassExtension.ExtensionConstructor constructor)
{
    public uint ExtraSize { get; init; } = extraSize;

    // public unsafe delegate void ExtensionConstructor<T>(T* Self) where T: unmanaged;
    public delegate void ExtensionConstructor(nint Self);
    public ExtensionConstructor Constructor { get; init; } = constructor;
    internal IHook<ExtensionConstructor>? ConstructorHook = null;
}