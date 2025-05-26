namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFMapProperty : IFProperty
{
    IFProperty KeyProp { get; }
    
    IFProperty ValueProp { get; }
}