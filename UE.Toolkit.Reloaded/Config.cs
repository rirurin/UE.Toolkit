using System.ComponentModel;
using UE.Toolkit.Reloaded.Common.GameConfigs;
using UE.Toolkit.Reloaded.Template.Configuration;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded;

public class Config : Configurable<Config>
{
    [DisplayName("Log Level")]
    [DefaultValue(LogLevel.Information)]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    [DisplayName("Log UObjects")]
    [DefaultValue(false)]
    public bool LogObjectsEnabled { get; set; } = false;

    [DisplayName("Log UDataTables")]
    [DefaultValue(false)]
    public bool LogTablesEnabled { get; set; } = false;

    [DisplayName("Game Config")]
    [Description("Use a specific game configuration in UE Toolkit.")]
    [DefaultValue(GameConfigVersion.Auto)]
    public GameConfigVersion GameConfig { get; set; } = GameConfigVersion.Auto;
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
}