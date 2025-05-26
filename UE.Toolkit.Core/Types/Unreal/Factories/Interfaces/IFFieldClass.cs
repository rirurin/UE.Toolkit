using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFFieldClass
{
    string Name { get; }
    ulong Id { get; }
    ulong CastFlags { get; }
    EClassFlags ClassFlags { get; }
    IFFieldClass SuperClass { get; }
    IFField DefaultObject { get; }
    nint FieldConstructor { get; }
}