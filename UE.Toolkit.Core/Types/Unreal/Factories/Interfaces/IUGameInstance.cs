namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUGameInstance : IUObject
{
    bool TryGetSubsystem(IUClass Class, out IUObject? Subsystem);
}