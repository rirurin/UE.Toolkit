using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using WorldType = UE.Toolkit.Core.Types.Unreal.UE5_4_4.WorldType;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Unreal;

public class UnrealState : IUnrealState
{
    private IUnrealFactory Factory;

    private nint GEngine;
   
    // See UEngine::GetCurrentPlayWorld
    public unsafe bool GetCurrentPlayWorld(out IUObject? TargetWorld)
    {
        TargetWorld = null;
        if (GEngine == nint.Zero) return false;
        var World = Factory.CreateUEngine(*(nint*)GEngine);
        foreach (var WorldContext in World.GetWorldList())
        {
            if (WorldContext.GetWorldType() == WorldType.Game && WorldContext.GetWorld() != nint.Zero)
            {
                TargetWorld = Factory.CreateUObject(WorldContext.GetWorld());
            }
        }
        return TargetWorld != null;
    }

    public UnrealState(IUnrealFactory _Factory)
    {
        Project.Scans.AddListener("GEngine", x => GEngine = x);
    }
}