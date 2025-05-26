using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.UE4_27_2;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Common.GameConfigs.Games;

public class UE4_27_2_P3R : UE5_4_4_ClairObscur
{
    public override string Id => "P3R";
    public override string UObject_PostLoadSubobjects => "40 53 48 83 EC 20 48 8B 41 ?? 48 8B D9 8B 90 ?? ?? ?? ?? C1 EA 17";
    public override string GUObjectArray => "48 8B 05 ?? ?? ?? ?? 48 8B 0C ?? 48 8D 04 ?? 48 85 C0 74 ?? 44 39 40 ?? 75 ?? F7 40 ?? 00 00 00 30 75 ?? 48 8B 00";
    public override Func<nint, nint> GUObjectArray_Result => result => ToolkitUtils.GetGlobalAddress(result + 3) - 0x10;
    public override string GFNamePool => "4C 8D 05 ?? ?? ?? ?? EB ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 4C 8B C0 C6 05 ?? ?? ?? ?? 01 48 8B 44 24 ?? 48 8B D3 48 C1 E8 20 8D 0C ?? 49 03 4C ?? ?? E8 ?? ?? ?? ?? 48 8B C3";
    public override string UEnum_GetDisplayNameTextByIndex => "48 89 5C 24 ?? 55 56 57 48 83 EC 30 48 8B FA 41 8B E8";
    public override IUnrealFactory Factory { get; } = new UnrealFactory();
}