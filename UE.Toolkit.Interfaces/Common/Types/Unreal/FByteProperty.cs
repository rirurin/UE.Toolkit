using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FByteProperty
{
    public FProperty Super;
    public UEnum* Enum;
}

[StructLayout(LayoutKind.Sequential)]
public struct FNumericProperty
{
    public FProperty Super;
}