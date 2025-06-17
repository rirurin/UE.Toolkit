using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealNames
{
    public UnrealNames()
    {
        Project.Scans.AddScan(nameof(FName.GFNamePool), result => FName.GFNamePool = (FNamePool*)result);
        
        Project.Scans.AddScanHook(nameof(FNameHelper_FindOrStoreString),
            (result, hooks) =>
            {
                FName.FNameHelper_FindOrStoreString =
                    hooks.CreateWrapper<FNameHelper_FindOrStoreString>(result, out _);
            });
        
        Project.Scans.AddScanHook(nameof(FName_Ctor_Wide),
            (result, hooks) =>
            {
                FName.FName_Constructor = hooks.CreateWrapper<FName_Ctor_Wide>(result, out _);
            });
    }
}