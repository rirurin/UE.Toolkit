using Reloaded.Hooks.ReloadedII.Interfaces;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.UE5_4_4;

namespace UE.Toolkit.Reloaded.Common.GameConfigs.Games;

// ReSharper disable once InconsistentNaming
public class UE5_4_4_ClairObscur : IGameConfig
{
    public virtual string Id => "Clair Obscur";
    public IUnrealFactory Factory { get; protected init; }

    public UE5_4_4_ClairObscur(IUnrealMemoryInternal allocator, IReloadedHooks _Hooks)
    {
        Factory = new UnrealFactory(allocator, _Hooks);
    }
}