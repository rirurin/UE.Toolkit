// ReSharper disable InconsistentNaming

using UE.Toolkit.Core.Types.Unreal.Factories;

namespace UE.Toolkit.Reloaded.Common.GameConfigs;

public interface IGameConfig
{
    string Id { get; }
    IUnrealFactory Factory { get; }
}