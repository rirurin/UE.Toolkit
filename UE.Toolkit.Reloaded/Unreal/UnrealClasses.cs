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
        TryHookCDO(returnClass);
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
        TryHookCDO(returnClass);
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
    
    private void TryHookCDO(UClass** ppClass)
    {
        if (_CreateDefaultObject != null) return;
        Log.Debug($"0x{*(nint*)(*ppClass) + CreateDefaultObjectOffset:x}");
        _CreateDefaultObject = Hooks.CreateHook<UClass_CreateDefaultObject>(UClass_CreateDefaultObjectImpl, 
            *(nint*)(*(nint*)(*ppClass) + CreateDefaultObjectOffset)).Activate();
    }

    private delegate nint UClass_CreateDefaultObject(nint pClass);
    private IHook<UClass_CreateDefaultObject>? _CreateDefaultObject = null;
    private uint CreateDefaultObjectOffset;

    private nint UClass_CreateDefaultObjectImpl(nint pClass)
    {
        var ClassDefaultObject = _CreateDefaultObject!.OriginalFunction(pClass);
        var Class = Factory.CreateUClass(pClass);
        foreach (var Property in Class.PropertyLink)
            FieldClasses.TryAdd(((FFieldClass*)Property.ClassPrivate.Ptr)->Name.ComparisonIndex.Value,
                new(Property.VTable, Property.ClassPrivate));
        return ClassDefaultObject;
    }
    
    #region IUnrealClasses implementation

    public void AddExtension<TObject>(uint ExtraSize, Action<ToolkitUObject<TObject>> callback)
        where TObject : unmanaged 
        => ClassExtensions.GetOrAdd(typeof(TObject).Name[1..], new ClassExtension(ExtraSize, 
            obj => callback(new((TObject*)*(nint*)obj))));

    public bool GetClassInfoFromClass<TObject>(out IUClass? Value) where TObject : unmanaged
        => GetClassInfoFromName(typeof(TObject).Name, out Value);

    public bool GetClassInfoFromName(string Name, out IUClass? Value)
    {
        Value = Classes.GetValueOrDefault(Name[1..]);
        return Value != null;
    }

    public void AddI8Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateI8<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddI16Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateI16<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddI32Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateI32<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddI64Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateI64<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddU8Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateU8<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddU16Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateU16<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddU32Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateU32<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddU64Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateU64<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddF32Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateF32<TObject>(out _, Name, Offset, PropertyVisibility.Public);
    public void AddF64Property<TObject>(string Name, int Offset) where TObject : unmanaged
        => PropertyFactory.CreateF64<TObject>(out _, Name, Offset, PropertyVisibility.Public);

    #endregion
    
    #region IUnrealClassesInternal implementation

    public bool GetFieldClassGlobal(FName Name, out FieldClassGlobal FieldClass)
        => FieldClasses.TryGetValue(Name.ComparisonIndex.Value, out FieldClass);
    
    #endregion

    private ConcurrentDictionary<string, ClassExtension> ClassExtensions = new();
    private ConcurrentDictionary<string, IUClass> Classes = new();
    private ConcurrentDictionary<ulong, FieldClassGlobal> FieldClasses = new();
    
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

        Project.Inis.UsingSetting<uint>(Constants.UnrealIniId, "CreateDefaultObject", nameof(UClass),
            value => CreateDefaultObjectOffset = value);
        
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