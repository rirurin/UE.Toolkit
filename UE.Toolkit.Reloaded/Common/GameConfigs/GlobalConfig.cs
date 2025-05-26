using UE.Toolkit.Reloaded.Common.GameConfigs.Games;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace UE.Toolkit.Reloaded.Common.GameConfigs;

public static class GameConfig
{
    public static IGameConfig Instance { get; private set; } = null!;

    public static void SetGame(string appId)
    {
        Instance = appId switch
        {
            "p3r.exe" => new UE4_27_2_P3R(),
            _ => new UE5_4_4_ClairObscur()
        };
    }
}
