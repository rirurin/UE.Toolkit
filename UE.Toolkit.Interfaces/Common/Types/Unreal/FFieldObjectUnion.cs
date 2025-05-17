using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct FFieldObjectUnion
{
    [FieldOffset(0x0)] public FField* Field;
    [FieldOffset(0x0)] public UObjectBase* Object;
}