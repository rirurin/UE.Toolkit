// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public struct TSet<InElementType>
    where InElementType : unmanaged
{
    public TSparseArray<TSetElementBase<InElementType>> Elements;
    public THashTable Hash;
    public int HashSize;
}

[StructLayout(LayoutKind.Sequential)]
public struct TSetElementBase<InElementType>
    where InElementType : unmanaged
{
    /** The element's value. */
    public InElementType Value;

    /** The id of the next element in the same hash bucket. */
    public int HashNextId; // FSetElementId

    /** The hash bucket that the element is currently linked to. */
    public int HashIndex;
}