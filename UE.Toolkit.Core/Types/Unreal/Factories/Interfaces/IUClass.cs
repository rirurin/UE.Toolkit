using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUClass : IUStruct
{
    IUClass? GetSuperClass();

    TMapDictionary<FName, Ptr<UFunction>> GetFunctionMap();
}