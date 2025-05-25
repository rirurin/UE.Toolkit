using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct FFieldObjectUnion
{
    [FieldOffset(0x0)] public FField* Field;
    [FieldOffset(0x0)] public UObjectBase* Object;
}