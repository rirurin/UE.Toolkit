using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FLazyObjectProperty
{
    public FObjectProperty Super;
}