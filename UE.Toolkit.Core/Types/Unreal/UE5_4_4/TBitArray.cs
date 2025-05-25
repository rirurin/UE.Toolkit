using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TBitArray
{
    public int* AllocatorInstance;
    public int NumBits;
    public int MaxBits;
}