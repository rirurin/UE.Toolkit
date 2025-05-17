using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FSetProperty
{
    public FProperty Super;
    public FProperty* ElementProp;
    //FScriptSetLayout SetLayout;
}