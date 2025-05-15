using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public unsafe struct UObjectBase
{
    public nint VTable;
    public EObjectFlags ObjectFlags;
    public int InternalIndex;
    public UClass* ClassPrivate;
    public FName NamePrivate;
    public UObjectBase* OuterPrivate;
}