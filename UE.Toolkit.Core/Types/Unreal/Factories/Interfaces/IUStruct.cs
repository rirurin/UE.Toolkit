using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUStruct : IUObject
{
    IUStruct? SuperStruct { get; }
    
    IEnumerable<IUField> Children { get; }
    
    IEnumerable<IFField> ChildProperties { get; }
    
    int PropertiesSize { get; }
    
    int MinAlignment { get; }
    
    TArray<byte> Script { get; }
    
    IEnumerable<IFProperty> PropertyLink { get; }
    
    IEnumerable<IFProperty> RefLink { get; }
    
    IEnumerable<IFProperty> DestructorLink { get; }
    
    IEnumerable<IFProperty> PostConstructLink { get; }
}