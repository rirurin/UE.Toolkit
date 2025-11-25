using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Explicit, Size = 0x1c0)]
public struct UGameInstance
{
    [FieldOffset(0x100)] public FSubsystemCollection_UGameInstanceSubsystem SubsystemCollection;
}

[StructLayout(LayoutKind.Explicit, Size = 0xc0)]
public struct FSubsystemCollection_UGameInstanceSubsystem
{
    [FieldOffset(0x10)] public TMap<Ptr<UClass>, Ptr<UObjectBase>> Subsystems;
}