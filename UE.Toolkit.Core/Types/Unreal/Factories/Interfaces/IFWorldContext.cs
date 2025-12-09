using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFWorldContext : IPtr
{
    public WorldType GetWorldType();
    public nint GetWorld();
}