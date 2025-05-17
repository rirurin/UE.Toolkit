using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FFieldPathProperty
{
    public FProperty Super;
    public FFieldClass* PropertyClass;
}