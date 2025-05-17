using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FMapProperty
{
    public FProperty Super;
    public FProperty* KeyProp;
    public FProperty* ValueProp;
    //FScriptMapLayout MapLayout;
    //EMapPropertyFlags MapFlags;
}