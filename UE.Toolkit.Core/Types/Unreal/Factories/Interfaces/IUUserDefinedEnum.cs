using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUUserDefinedEnum : IUEnum
{
    public TMap<FName, FText> DisplayNameMap { get; }
}