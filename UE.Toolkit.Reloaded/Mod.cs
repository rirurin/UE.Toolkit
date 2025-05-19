#if DEBUG
using System.Diagnostics;
#endif
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using UE.Toolkit.Core.Types.Unreal;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.Template;
using UE.Toolkit.Reloaded.DataTables;
using UE.Toolkit.Reloaded.Unreal;

namespace UE.Toolkit.Reloaded;

public class Mod : ModBase, IExports
{
#pragma warning disable CA2211
    public static Config Config = null!;
#pragma warning restore CA2211
    
    private readonly IModLoader _modLoader;
    private readonly IReloadedHooks? _hooks;
    private readonly ILogger _log;
    private readonly IMod _owner;

    private readonly IModConfig _modConfig;

    private readonly UnrealNames _names;
    private readonly UnrealMemory _memory;
    private readonly UnrealObjects _objects;
    private readonly DataTablesService _tables;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _log = context.Logger;
        _owner = context.Owner;
        Config = context.Configuration;
        _modConfig = context.ModConfig;
#if DEBUG
        Debugger.Launch();
#endif
        Project.Initialize(_modConfig, _modLoader, _log, true);
        Log.LogLevel = Config.LogLevel;

        _names = new();
        _objects = new();
        _tables = new();
        _memory = new();
        
        _modLoader.AddOrReplaceController<IUnrealMemory>(_owner, _memory);
        _modLoader.AddOrReplaceController<IDataTables>(_owner, _tables);
        _modLoader.AddOrReplaceController<IUnrealObjects>(_owner, _objects);
    }

    #region Standard Overrides

    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        Config = configuration;
        _log.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        Log.LogLevel = Config.LogLevel;
    }

    #endregion

    #region For Exports, Serialization etc.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod()
    {
    }
#pragma warning restore CS8618

    #endregion

    public Type[] GetTypes() => [typeof(IDataTables), typeof(IUnrealObjects), typeof(UObjectBase)];
}