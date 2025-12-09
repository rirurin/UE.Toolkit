using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct FPropertyParamsBase
{
    public nint NameUTF8;
    public nint RepNotifyFuncUTF8;
    public EPropertyFlags PropertyFlags;
    public EPropertyGenFlags PropertyGenFlags;
    public EObjectFlags ObjectFlags;
    public nint SetterFunc;
    public nint GetterFunc;
    public ushort ArrayDim;
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
    public nint SetterFunc;
    public nint GetterFunc;
    public ushort ArrayDim;
    public ushort Offset;
}

[StructLayout(LayoutKind.Sequential)]
public struct FGenericPropertyParams
{
    public FPropertyParamsBaseWithOffset Super;
}