using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct TLazyObjectPtr<TObject>
    where TObject : unmanaged
{
    public FPersistentObjectPtr Super;
}

[StructLayout(LayoutKind.Sequential)]
public struct FLazyObjectPtr
{
    public FPersistentObjectPtr Super;
}