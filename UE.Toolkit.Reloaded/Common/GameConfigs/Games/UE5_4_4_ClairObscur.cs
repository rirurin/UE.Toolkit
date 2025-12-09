using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.UE5_4_4;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Unreal;

namespace UE.Toolkit.Reloaded.Common.GameConfigs.Games;

// ReSharper disable once InconsistentNaming
public class UE5_4_4_ClairObscur : IGameConfig
{
    public virtual string Id => "Clair Obscur";
    public virtual IUnrealFactory Factory { get; } = new UnrealFactory();
    public virtual IUnrealMemory Memory { get; } = new UnrealMemory();
}