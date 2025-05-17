using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FObjectProperty
{
    public FProperty Super;
    public UClass* PropertyClass;
}