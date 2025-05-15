// ReSharper disable InconsistentNaming

using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Unreal;

public class UnrealMemory : IUnrealMemory
{
    internal delegate nint FMemory_Malloc(nint count, int alignment = 0);
    internal static SHFunction<FMemory_Malloc>? _FMemory_Malloc;
    
    internal delegate void FMemory_Free(nint original);
    internal static SHFunction<FMemory_Free>? _FMemory_Free;

    public UnrealMemory()
    {       
        _FMemory_Malloc = new("48 89 5C 24 ?? 57 48 83 EC 20 48 8B F9 8B DA 48 8B 0D ?? ?? ?? ?? 48 85 C9 75 ?? E8");
        _FMemory_Free = new("48 85 C9 74 ?? 53 48 83 EC 20 48 8B D9 48 8B 0D");
    }
    
    public nint FMemoryMalloc(nint count, int alignment = 0) => _FMemory_Malloc!.Wrapper(count, alignment);
    
    public void FMemoryFree(nint original) => _FMemory_Free!.Wrapper(original);
}