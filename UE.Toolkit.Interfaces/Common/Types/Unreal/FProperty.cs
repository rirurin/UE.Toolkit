using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 0x70)]
public unsafe struct FProperty
    // FInt8Property, FInt16Property, FIntProperty, FInt64Property
    // FUint8Property, FUint16Property, FUintProperty, FUint64Property
    // FFloatProperty, FDoubleProperty, FNameProperty, FStrProperty
{
    public FField Super;
    public int ArrayDim;
    public int ElementSize;
    public EPropertyFlags PropertyFlags;
    public ushort RepIndex;
    public byte BlueprintReplicationCondition;
    public int Offset_Internal;
    public FProperty* PropertyLinkNext;
    public FProperty* NextRef;
    public FProperty* DestructorLinkNext;
    public FProperty* PostConstructLinkNext;
    public FName RepNotifyFunc;
}