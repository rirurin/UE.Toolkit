using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Explicit, Size = 0x90)]
public unsafe struct FStaticConstructObjectParameters
{
    /** The class of the object to create */
    [FieldOffset(0x0)] public UClass* Class;
    
    /** The object to create this object within (the Outer property for the new object will be set to the value specified here). */
    [FieldOffset(0x8)] public UObjectBase* Outer;
    
    /** The name to give the new object.If no value(NAME_None) is specified, the object will be given a unique name in the form of ClassName_#. */
    [FieldOffset(0x10)] public FName Name;
    
    /** The ObjectFlags to assign to the new object. some flags can affect the behavior of constructing the object. */
    [FieldOffset(0x18)] public EObjectFlags SetFlags;
    
    /** The InternalObjectFlags to assign to the new object. some flags can affect the behavior of constructing the object. */
    [FieldOffset(0x1c)] public EInternalObjectFlags InternalSetFlags;
}