using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFPropertyParams : IPtr
{
    string Name { get; }
    EPropertyFlags PropertyFlags { get; }
    EPropertyGenFlags GenFlags { get; }
    EObjectFlags ObjectFlags { get; }
}