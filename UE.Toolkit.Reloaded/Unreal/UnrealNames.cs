using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Reloaded.Common.GameConfigs;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealNames
{
    //private readonly SHFunction<FNameHelper_FindOrStoreString> _FNameHelper_FindOrStoreString;
    
    public UnrealNames()
    {
        ScanHooks.Add(nameof(FName.GFNamePool),
            GameConfig.Instance.GFNamePool,
            (_, result) => FName.GFNamePool = (FNamePool*)ToolkitUtils.GetGlobalAddress(result + 3));
        
        ScanHooks.Add(nameof(FNameHelper_FindOrStoreString), GameConfig.Instance.FNameHelper_FindOrStoreString,
            (hooks, result) =>
            {
                FName.FNameHelper_FindOrStoreString =
                    hooks.CreateWrapper<FNameHelper_FindOrStoreString>(result, out _);
            });
        ScanHooks.Add(nameof(FName_Ctor_Wide), GameConfig.Instance.FName_Ctor_Wide,
            (hooks, result) =>
            {
                FName.FName_Constructor = hooks.CreateWrapper<FName_Ctor_Wide>(result, out _);
            });
    }
}