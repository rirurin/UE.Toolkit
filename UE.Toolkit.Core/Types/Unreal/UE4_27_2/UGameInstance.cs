using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE4_27_2;

[StructLayout(LayoutKind.Explicit, Size = 0x1a8)]
public struct UGameInstance
{
    [FieldOffset(0xe0)] public FSubsystemCollection_UGameInstanceSubsystem SubsystemCollection;
}

[StructLayout(LayoutKind.Explicit, Size = 0xc8)]
public struct FSubsystemCollection_UGameInstanceSubsystem
{
    [FieldOffset(0x10)] public UE5_4_4.TMap<Ptr<UClass>, Ptr<UObjectBase>> Subsystems;
}