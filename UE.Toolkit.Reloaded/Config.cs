using UE.Toolkit.Reloaded.Template.Configuration;
using System.ComponentModel;

namespace UE.Toolkit.Reloaded.Configuration;

public class Config : Configurable<Config>
{
    [DisplayName("Log Level")]
    [DefaultValue(LogLevel.Information)]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    [DisplayName("Log UObjects")]
    public bool LogObjectsEnabled { get; set; } = false;

    [DisplayName("Log UDataTables")]
    public bool LogTablesEnabled { get; set; } = false;
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
}