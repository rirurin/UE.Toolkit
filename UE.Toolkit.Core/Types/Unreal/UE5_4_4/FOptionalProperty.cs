using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FOptionalProperty
{
    public FProperty Super;
    public FProperty* ValueProperty;
}