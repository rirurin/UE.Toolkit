// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMap<InKeyType, InValueType>
    where InKeyType : unmanaged
    where InValueType : unmanaged
{
    // From TArray
    public TMapElement<InKeyType, InValueType>* Elements; // Data
    public int MapNum; // ArrayNum
    public int MapMax; // ArrayMax

    // From TSparseArray
    // public TBitArray AllocationFlags;
    // public int FirstFreeIndex;
    // public int NumFreeIndices;
    
    // From TSet (probably)
    
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMapElement<InKeyType, InValueType>
    where InKeyType : unmanaged
    where InValueType : unmanaged
{
    public InKeyType Key;
    public InValueType* Value;
    
    // From TSetElementBase
    /** The id of the next element in the same hash bucket. */
    public int HashNextId; // FSetElementId

    /** The hash bucket that the element is currently linked to. */
    public int HashIndex;
}
