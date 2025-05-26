using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFProperty : IFField
{
    int ArrayDim { get; }
    int ElementSize { get; }
    EPropertyFlags PropertyFlags { get; }
    ushort RepIndex { get; }
    byte BlueprintReplicationCondition { get; }
    int Offset_Internal { get; }
    IEnumerable<IFProperty> PropertyLinkNext { get; }
    IEnumerable<IFProperty> NextRef { get; }
    IEnumerable<IFProperty> DestructorLinkNext { get; }
    IEnumerable<IFProperty> PostConstructLinkNext { get; }
    string RepNotifyFunc { get; }
}