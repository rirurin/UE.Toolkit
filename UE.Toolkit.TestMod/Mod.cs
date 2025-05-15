#if DEBUG
using System.Diagnostics;
#endif
using System.Runtime.InteropServices;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Interfaces.Common.Types.Unreal;
using UE.Toolkit.TestMod.Template;
using UE.Toolkit.TestMod.Configuration;

namespace UE.Toolkit.TestMod;

public unsafe class Mod : ModBase
{
    private readonly IModLoader _modLoader;
    private readonly IReloadedHooks? _hooks;
    private readonly ILogger _log;
    private readonly IMod _owner;

    private Config _config;
    private readonly IModConfig _modConfig;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _log = context.Logger;
        _owner = context.Owner;
        _config = context.Configuration;
        _modConfig = context.ModConfig;
#if DEBUG
        Debugger.Launch();
#endif
        Project.Initialize(_modConfig, _modLoader, _log, true);
        Log.LogLevel = _config.LogLevel;

        _modLoader.GetController<IDataTables>().TryGetTarget(out var dt);
        _modLoader.GetController<IUnrealObjects>().TryGetTarget(out var objects);
        dt.OnDataTableChanged<S_LoadingScreenTips>("DT_LoadingScreenTips", table =>
        {
            foreach (var row in table)
            {
                var tips = row.Instance->Value;
                for (int i = 0; i < tips->Tips.ArrayNum; i++)
                {
                    tips->Tips.AllocatorInstance[i] = *objects.CreateFText("Brother may i have some oats?\nno.\nI am starving, brother.\nAs am i, brother. The tall skinny figure has thrown the oats at me. ME, BROTHER. i believe they have taken a liking to me.\nNo brother, I have seen this before. I have observed many things. From the roaring beasts that the tall skinny figures crawl inside of to travel far beyond the horizon, to how the figure weeped when the other had fallen into a deep sleep. And from my experiences I have learned that they will give extra oats to one of us before taking them into the shed of no return.. They will do terrible things in that shed, brother.");
                }
            }
            
            Log.Information(table.Instance->BaseObj.NamePrivate.ToString());
        });
    }

    #region Standard Overrides

    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        _config = configuration;
        _log.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        Log.LogLevel = _config.LogLevel;
    }

    #endregion

    #region For Exports, Serialization etc.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod()
    {
    }
#pragma warning restore CS8618

    #endregion
}

[StructLayout(LayoutKind.Sequential)]
public struct S_LoadingScreenTips
{
    private byte GoldenPathMin;
    private byte GoldenPathMax;
    public TArray<FText> Tips;
}