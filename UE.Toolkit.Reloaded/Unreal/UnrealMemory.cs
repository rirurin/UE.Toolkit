using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Common.GameConfigs;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public class UnrealMemory : IUnrealMemory
{
    internal delegate nint FMemory_Malloc(nint count, int alignment = 0);
    internal static SHFunction<FMemory_Malloc>? _FMemory_Malloc;
    
    internal delegate void FMemory_Free(nint original);
    internal static SHFunction<FMemory_Free>? _FMemory_Free;

    public UnrealMemory()
    {       
        _FMemory_Malloc = new(GameConfig.Instance.FMemory_Malloc);
        _FMemory_Free = new(GameConfig.Instance.FMemory_Free);
    }
    
    public nint Malloc(nint count, int alignment = 0) => _FMemory_Malloc!.Wrapper(count, alignment);
    
    public void Free(nint original) => _FMemory_Free!.Wrapper(original);
}