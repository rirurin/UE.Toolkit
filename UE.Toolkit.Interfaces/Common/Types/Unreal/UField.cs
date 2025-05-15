using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x30, Pack = 8)]
public unsafe struct UField
{
    public UObjectBase Obj;
    public UField* Next;
}