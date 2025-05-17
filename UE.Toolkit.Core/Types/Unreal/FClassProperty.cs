using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FClassProperty
{
    public FObjectProperty Super;
    public UClass* MetaClass;
}