#if DEBUG
using System.Diagnostics;
#endif
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Interfaces.ObjectWriters;
using UE.Toolkit.Reloaded.Common.GameConfigs;
using UE.Toolkit.Reloaded.Template;
using UE.Toolkit.Reloaded.DataTables;
using UE.Toolkit.Reloaded.ObjectWriters;
using UE.Toolkit.Reloaded.Toolkit;
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

    private readonly IUnrealFactory _factory;
    private readonly UnrealNames _names;
    private readonly UnrealMemory _memory;
    private readonly UnrealObjects _objects;
    private readonly DataTablesService _tables;
    private readonly TypeRegistry _typeRegistry;
    private readonly ObjectWriterService _writer;
    private readonly ToolkitApi _toolkit;

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
        
        // Setup game patterns and Unreal factory.
        GameConfig.SetGame(_modLoader.GetAppConfig().AppId);

        _factory = GameConfig.Instance.Factory;
        _names = new();
        _objects = new(_factory);
        _tables = new();
        _memory = new();
        _typeRegistry = new();
        _writer = new(_typeRegistry, _objects, _tables);
        _toolkit = new(_writer);
        
        _modLoader.AddOrReplaceController<IUnrealMemory>(_owner, _memory);
        _modLoader.AddOrReplaceController<IDataTables>(_owner, _tables);
        _modLoader.AddOrReplaceController<IUnrealObjects>(_owner, _objects);
        _modLoader.AddOrReplaceController<ITypeRegistry>(_owner, _typeRegistry);
        _modLoader.AddOrReplaceController<IToolkit>(_owner, _toolkit);
        _modLoader.AddOrReplaceController(_owner, _factory);
        
        _modLoader.ModLoaded += OnModLoaded;
    }

    private void OnModLoaded(IModV1 mod, IModConfigV1 modConfig)
    {
        var modDir = _modLoader.GetDirectoryForModId(modConfig.ModId);
        var toolkitDir = Path.Join(modDir, "ue-toolkit");
        if (Directory.Exists(toolkitDir)) _toolkit.AddToolkitFolder(toolkitDir);
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

    public Type[] GetTypes() => [typeof(IDataTables), typeof(IUnrealObjects), typeof(IToolkit), typeof(ITypeRegistry), typeof(UObjectBase), typeof(IUnrealFactory)];
}