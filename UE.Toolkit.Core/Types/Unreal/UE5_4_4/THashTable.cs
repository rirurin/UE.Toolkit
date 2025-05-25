// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct THashTable
{
    public FScriptContainerElement* Hash;
    public FScriptContainerElement* NextIndex;
    public uint HashMask;
    public uint IndexSize;
}