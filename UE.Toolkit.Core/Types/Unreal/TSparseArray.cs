// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct TSparseArray<InElementType>
    where InElementType : unmanaged
{
    public TArray<InElementType> Data;
    
    // Unverified.
    public TBitArray AllocationFlags;
    public int FirstFreeIndex;
    public int NumFreeIndices;
}