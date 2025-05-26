namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUField : IUObject
{
    IUField? Next { get; }
}