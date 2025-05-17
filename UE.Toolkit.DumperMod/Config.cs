using UE.Toolkit.DumperMod.Template.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UE.Toolkit.DumperMod.Configuration;

public class Config : Configurable<Config>
{
    [DisplayName("Log Level")]
    [DefaultValue(LogLevel.Information)]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    [DisplayName("Dump Mode")]
    [DefaultValue(DumpFileMode.SingleFile)]
    public DumpFileMode Mode { get; set; } = DumpFileMode.SingleFile;

    [DisplayName("File Namespace")]
    [DefaultValue("")]
    public string FileNamespace { get; set; } = string.Empty;
    
    [DisplayName("File Usings (Separate with ; )")]
    [DefaultValue("")]
    public string FileUsings { get; set; } = string.Empty;

    [DisplayName("Single-File File Name")]
    [DefaultValue("")]
    public string SingleFileOutputName { get; set; } = string.Empty;
}

public enum DumpFileMode
{
    [Display(Name = "Single-File")]
    SingleFile,
    
    [Display(Name = "File Per Type")]
    FilePerType,
    
    [Display(Name = "File Per Module")]
    FilePerModule,
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
}