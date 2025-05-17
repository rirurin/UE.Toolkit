using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FOptionalProperty
{
    public FProperty Super;
    public FProperty* ValueProperty;
}