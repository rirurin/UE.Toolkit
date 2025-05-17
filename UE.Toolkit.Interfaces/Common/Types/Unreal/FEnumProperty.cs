using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FEnumProperty
{
    public FProperty Super;
    public FProperty* UnderlyingProp; //FNumericProperty*
    public UEnum* Enum;
}