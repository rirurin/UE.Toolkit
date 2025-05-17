using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct FArrayProperty
{
    public FProperty Super;
    public EArrayPropertyFlags ArrayFlags;
    public FProperty* Inner;
}

public enum EArrayPropertyFlags : byte
{
    None,
    UsesMemoryImageAllocator
}
