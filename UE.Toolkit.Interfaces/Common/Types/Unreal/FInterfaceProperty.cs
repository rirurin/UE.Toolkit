using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FInterfaceProperty
{
    public FProperty Super;
    public UClass* InterfaceClass;
}