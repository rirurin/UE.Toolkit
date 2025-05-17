using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FFieldPathProperty
{
    public FProperty Super;
    public FFieldClass* PropertyClass;
}