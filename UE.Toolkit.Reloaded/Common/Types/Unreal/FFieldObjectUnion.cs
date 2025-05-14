using System.Runtime.InteropServices;

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FFieldObjectUnion
{
    public FField* Field;
    public UObjectBase* Object;
}