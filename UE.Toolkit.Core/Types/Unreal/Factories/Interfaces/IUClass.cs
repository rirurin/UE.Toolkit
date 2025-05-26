using UE.Toolkit.Core.Types.Unreal.Factories.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUClass : IUStruct
{
    IUClass? GetSuperClass();
}