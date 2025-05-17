using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FPersistentObjectPtr
{
    public FWeakObjectPtr WeakPtr;
    public Guid ObjectId;
}

[StructLayout(LayoutKind.Sequential)]
public struct TPersistentObjectPtr<TObject> where TObject : unmanaged
{
    public FWeakObjectPtr WeakPtr;
    public Guid ObjectId;
}