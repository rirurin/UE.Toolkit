using Reloaded.Hooks.ReloadedII.Interfaces;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.UE4_27_2;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Common.GameConfigs.Games;

//public class UE4_27_2_P3R : UE5_4_4_ClairObscur
public class UE4_27_2_P3R : IGameConfig
{
    public virtual string Id => "P3R";
    public IUnrealFactory Factory { get; protected init; }

    public UE4_27_2_P3R(IUnrealMemoryInternal Allocator, IReloadedHooks _Hooks)
    {
        Factory = new UnrealFactory(Allocator, _Hooks);
    }
}