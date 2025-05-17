using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FOptionalProperty
{
    public FProperty Super;
    public FProperty* ValueProperty;
}