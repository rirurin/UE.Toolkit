// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FScriptInterface
{/**
     * A pointer to a UObject that implements an interface.
     */
    public UObjectBase* ObjectPointer;

    /**
     * For native interfaces, pointer to the location of the interface object within the UObject referenced by ObjectPointer.
     */
    public void* InterfacePointer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TScriptInterface<TInterface>
    where TInterface : unmanaged
{
    /**
     * A pointer to a UObject that implements an interface.
     */
    public UObjectBase* ObjectPointer;

    /**
     * For native interfaces, pointer to the location of the interface object within the UObject referenced by ObjectPointer.
     */
    public TInterface* InterfacePointer;
}