using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.UE4_27_2;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Common.GameConfigs.Games;

public class UE4_27_2_P3R : UE5_4_4_ClairObscur
{
    public override string Id => "P3R";
    public override IUnrealFactory Factory { get; } = new UnrealFactory();
}