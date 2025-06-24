using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct FTopLevelAssetPath
{
    /** Name of the package containing the asset e.g. /Path/To/Package */
    public FName PackageName;
    
    /** Name of the asset within the package e.g. 'AssetName' */
    public FName AssetName;
}