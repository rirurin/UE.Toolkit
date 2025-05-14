using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TArray<T> where T : unmanaged
{
    public T* AllocatorInstance;
    public int ArrayNum;
    public int ArrayMax;
}