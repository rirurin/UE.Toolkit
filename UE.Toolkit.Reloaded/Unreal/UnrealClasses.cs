using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
// using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Common;

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
    }

    private void TryExtendClass(nint pClassName, nint classCtor, ref uint Size)
    {
        var ClassName = Marshal.PtrToStringUni(pClassName);
        if (ClassName == null || !ClassExtensions.TryGetValue(ClassName, out var Extension)) return;
        var OldSize = Size;
        Size += Extension.ExtraSize;
        if (classCtor != nint.Zero)
        {
            Extension.ConstructorHook = FollowThunkToGetAppropriateHook(classCtor, Extension.Constructor);
        }
        Log.Debug($"{ClassName} size: {OldSize} -> {Size}");
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
        Log.Debug($"Transformed address: {addr:X} -> {transformed:X}");
        retHook = Hooks.CreateHook(ctorHookReal, (long)transformed).Activate();
        return retHook;
    }

    public void AddExtension<TObject>(uint ExtraSize, Action<ToolkitUObject<TObject>> callback)
        where TObject : unmanaged 
        => ClassExtensions.GetOrAdd(typeof(TObject).Name[1..], new ClassExtension(ExtraSize, 
            obj => callback(new((TObject*)*(nint*)obj)))); 

    private ConcurrentDictionary<string, ClassExtension> ClassExtensions = new();
    private IReloadedHooks Hooks;
    private ResolveAddress Address;

    public UnrealClasses(IReloadedHooks _Hooks, ResolveAddress _Address)
    {
        Hooks = _Hooks;
        Address = _Address;
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