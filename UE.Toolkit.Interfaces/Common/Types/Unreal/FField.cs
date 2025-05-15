using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct FField
{
    public nint VTable;
    public FFieldClass* ClassPrivate;
    public FFieldObjectUnion* Owner;
    public FField* Next;
    public FName NamePrivate;
    public EObjectFlags FlagsPrivate;
}