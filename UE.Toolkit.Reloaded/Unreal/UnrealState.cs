using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using WorldType = UE.Toolkit.Core.Types.Unreal.UE5_4_4.WorldType;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Common.GameConfigs;

namespace UE.Toolkit.Reloaded.Unreal;

public class UnrealState : IUnrealState
{
    private IUnrealFactory Factory;
    private IUnrealClasses Classes;

    private nint GEngine;
   
    // Based on UE's implementation of UEngine::GetCurrentPlayWorld
    public unsafe bool GetCurrentPlayWorld(out IUObject? TargetWorld)
    {
        TargetWorld = null;
        if (GEngine == nint.Zero) return false;
        var UEngine = Factory.CreateUEngine(*(nint*)GEngine);
        IUObject? NoneWorld = null;
        foreach (var WorldContext in UEngine.GetWorldList())
        {
            if (WorldContext.GetWorld() == nint.Zero) continue;
            switch (WorldContext.GetWorldType())
            {
                case WorldType.Game:
                    TargetWorld = Factory.CreateUObject(WorldContext.GetWorld());
                    break;
                case WorldType.None:
                    NoneWorld = Factory.CreateUObject(WorldContext.GetWorld());
                    break;
            }
        }
        // For Persona 3 Reload, there is only one active world containing a list of streamed sublevels.
        // This may be true for other games as well, I haven't checked
        if (GameConfig.Instance.Id == "P3R")
        {
            TargetWorld = NoneWorld;
        }
        return TargetWorld != null;
    }

    public bool GetSubsystem<TSubsystem>(IUGameInstance? GameInstance, out IUObject? Subsystem)
        where TSubsystem : unmanaged
    {
        if (!Classes.GetClassInfoFromClass<TSubsystem>(out var Class))
        {
            Subsystem = null;
            return false;
        }
        return GetSubsystem(GameInstance, Class, out Subsystem);
    }

    public bool GetSubsystem(IUGameInstance? GameInstance, IUClass? SubsystemType, out IUObject? SubsystemObj)
        => GameInstance.TryGetSubsystem(SubsystemType, out SubsystemObj);

    public UnrealState(IUnrealFactory _Factory, IUnrealClasses _Classes)
    {
        Factory = _Factory;
        Classes = _Classes;
        Project.Scans.AddListener("GEngine", x => GEngine = x);
    }
}