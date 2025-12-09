namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUClass : IUStruct
{
    IUClass? GetSuperClass();

    IUFunction? GetFunction(string Name);
}