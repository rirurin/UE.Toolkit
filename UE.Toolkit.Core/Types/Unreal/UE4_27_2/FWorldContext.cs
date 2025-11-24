using System.Runtime.InteropServices;
using WorldType = UE.Toolkit.Core.Types.Unreal.UE5_4_4.WorldType;

namespace UE.Toolkit.Core.Types.Unreal.UE4_27_2;

[StructLayout(LayoutKind.Explicit, Size = 0x288)]
public unsafe struct FWorldContext
{
    [FieldOffset(0x10a)] public WorldType WorldType;
    [FieldOffset(0x280)] public UWorld* ThisCurrentWorld;
}