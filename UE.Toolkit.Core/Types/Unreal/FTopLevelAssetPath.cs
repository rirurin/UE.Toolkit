using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FTopLevelAssetPath
{
    /** Name of the package containing the asset e.g. /Path/To/Package */
    public FName PackageName;
    
    /** Name of the asset within the package e.g. 'AssetName' */
    public FName AssetName;
}