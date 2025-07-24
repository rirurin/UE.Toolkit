// ReSharper disable InconsistentNaming

using Reloaded.Hooks.ReloadedII.Interfaces;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.Factories;

namespace UE.Toolkit.Reloaded.Common.GameConfigs;

public interface IGameConfig
{
    string Id { get; }
    IUnrealFactory Factory { get; }
}