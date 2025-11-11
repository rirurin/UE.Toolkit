using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct FBoolProperty
{
    public FProperty Super;
    public byte FieldSize;
    public byte ByteOffset;
    public byte ByteMask;
    public byte FieldMask;
}