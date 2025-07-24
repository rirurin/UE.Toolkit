using Reloaded.Hooks.ReloadedII.Interfaces;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Reloaded.Common.GameConfigs.Games;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace UE.Toolkit.Reloaded.Common.GameConfigs;

public static class GameConfig
{
    private static readonly Dictionary<GameConfigVersion, Func<IUnrealMemoryInternal, IReloadedHooks, IGameConfig>> _gameConfigs = new()
    {
        [GameConfigVersion.UE5_4_4_ClairObscur] = (alloc, hooks) => new UE5_4_4_ClairObscur(alloc, hooks),
        [GameConfigVersion.UE4_27_2_P3R] = (alloc, hooks) => new UE4_27_2_P3R(alloc, hooks),
    };
    
    public static IGameConfig Instance { get; private set; } = null!;

    public static void SetGame(string appId, IUnrealMemoryInternal Allocator, IReloadedHooks Hooks)
    {
        if (Mod.Config.GameConfig != GameConfigVersion.Auto)
        {
            Instance = _gameConfigs[Mod.Config.GameConfig](Allocator, Hooks);
            return;
        }
        
        Instance = appId switch
        {
            "p3r.exe" => new UE4_27_2_P3R(Allocator, Hooks),
            _ => new UE5_4_4_ClairObscur(Allocator, Hooks)
        };
        
        Log.Information($"Game config set to: {Instance.Id} (App ID: {appId})");
    }
}
