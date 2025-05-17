// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct THashTable
{
    public FScriptContainerElement* Hash;
    public FScriptContainerElement* NextIndex;
    public uint HashMask;
    public uint IndexSize;
}