namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFArrayProperty : IFProperty
{
    IFProperty Inner { get; }
}