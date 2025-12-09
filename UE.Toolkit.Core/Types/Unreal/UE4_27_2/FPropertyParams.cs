using System.Runtime.InteropServices;
using EObjectFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EObjectFlags;
using EPropertyFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EPropertyFlags;
using EPropertyGenFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EPropertyGenFlags;

namespace UE.Toolkit.Core.Types.Unreal.UE4_27_2;

[StructLayout(LayoutKind.Sequential)]
public struct FPropertyParamsBase
{
    public nint NameUTF8;
    public nint RepNotifyFuncUTF8;
    public EPropertyFlags PropertyFlags;
    public EPropertyGenFlags PropertyGenFlags;
    public EObjectFlags ObjectFlags;
    public int ArrayDim;
}

[StructLayout(LayoutKind.Sequential)]
public struct FPropertyParamsBaseWithOffset
{
    // public FPropertyParamsBase Super;
    public nint NameUTF8;
    public nint RepNotifyFuncUTF8;
    public EPropertyFlags PropertyFlags;
    public EPropertyGenFlags PropertyGenFlags;
    public EObjectFlags ObjectFlags;
    public int ArrayDim;
    public int Offset;
}

[StructLayout(LayoutKind.Sequential)]
public struct FGenericPropertyParams
{
    public FPropertyParamsBaseWithOffset Super;
}