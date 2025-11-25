using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Explicit, Size = 0x80)]
public unsafe struct FActorSpawnParameters
{
    /* A name to assign as the Name of the Actor being spawned. If no value is specified, the name of the spawned Actor will be automatically generated using the form [Class]_[Number]. */
    [FieldOffset(0x0)] public FName Name;
    
    /* The parent component to set the Actor in. */
    [FieldOffset(0x34)] public EObjectFlags ObjectFlags;
}