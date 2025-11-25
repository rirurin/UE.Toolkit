using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFStaticConstructObjectParameters : IPtr
{
    void SetParams(IUClass Class, IUObject? Owner, FName Name);
}