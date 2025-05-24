using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealNames
{
    //private readonly SHFunction<FNameHelper_FindOrStoreString> _FNameHelper_FindOrStoreString;
    
    public UnrealNames()
    {
        ScanHooks.Add(nameof(FName.GFNamePool),
            "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 4C 8B C0 C6 05 ?? ?? ?? ?? 01 8B D3 0F B7 C3 89 44 24",
            (_, result) => FName.GFNamePool = (FNamePool*)ToolkitUtils.GetGlobalAddress(result + 3));
        
        ScanHooks.Add(nameof(FNameHelper_FindOrStoreString), "48 89 74 24 ?? 57 48 83 EC 60 81 79 ?? 00 04 00 00",
            (hooks, result) =>
            {
                FName.FNameHelper_FindOrStoreString =
                    hooks.CreateWrapper<FNameHelper_FindOrStoreString>(result, out _);
            });
    }
}