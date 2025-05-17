using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FStructProperty
{
    public FProperty Super;
    public UScriptStruct* Struct;
}