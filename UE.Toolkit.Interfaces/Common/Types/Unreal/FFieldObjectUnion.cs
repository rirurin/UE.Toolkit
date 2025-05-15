using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FFieldObjectUnion
{
    public FField* Field;
    public UObjectBase* Object;
}