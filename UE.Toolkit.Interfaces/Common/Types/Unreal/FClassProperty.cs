using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FClassProperty
{
    public FObjectProperty Super;
    public UClass* MetaClass;
}