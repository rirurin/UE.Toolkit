namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFEnumProperty : IFProperty
{
    IUEnum Enum { get; }
    
    IFProperty UnderlyingProp { get; }
}