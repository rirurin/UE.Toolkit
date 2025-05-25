using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct FSoftObjectPath
{
    /** Asset path, patch to a top level object in a package. This is /package/path.assetname */
    public FTopLevelAssetPath AssetPath;

    /** Optional FString for subobject within an asset. This is the sub path after the : */
    public FString SubPathString;
}