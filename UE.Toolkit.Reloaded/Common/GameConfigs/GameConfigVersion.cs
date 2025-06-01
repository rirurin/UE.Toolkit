using System.ComponentModel.DataAnnotations;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Common.GameConfigs;

public enum GameConfigVersion
{
    Auto,
    
    [Display(Name = "UE 5.4.4 (Clair Obscur)")]
    UE5_4_4_ClairObscur,
    
    [Display(Name = "UE 4.27.2 (Persona 3 Reload)")]
    UE4_27_2_P3R,
}