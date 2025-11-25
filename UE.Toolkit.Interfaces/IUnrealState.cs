using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

namespace UE.Toolkit.Interfaces;

public interface IUnrealState
{
    /// <summary>
    /// Try to get the currently active world. It's recommended to use a World Context Object - a UObject that
    /// belongs to a particular world and call GetWorld() from there. This is useful in cases where there is no
    /// World Context Object.
    /// </summary>
    /// <param name="TargetWorld">The target world.</param>
    /// <returns>If a target world could be found.</returns>
    public bool GetCurrentPlayWorld(out IUObject? TargetWorld);

    public bool GetSubsystem<TSubsystem>(IUGameInstance? GameInstance, out IUObject? Subsystem) 
        where TSubsystem : unmanaged;

    public bool GetSubsystem(IUGameInstance? GameInstance, IUClass? SubsystemType, out IUObject? SubsystemObj);
}