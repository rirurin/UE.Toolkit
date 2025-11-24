using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.UE4_27_2;

[StructLayout(LayoutKind.Explicit, Size = 0xd20)]
public struct UEngine
{
    [FieldOffset(0xc38)] public TArray<Ptr<FWorldContext>> WorldList;
}