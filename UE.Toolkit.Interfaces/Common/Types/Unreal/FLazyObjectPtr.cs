using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

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