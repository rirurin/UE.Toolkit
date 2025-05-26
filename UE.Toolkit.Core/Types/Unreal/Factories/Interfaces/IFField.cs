using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFField : IPtr
{
    nint VTable { get; }
    IFFieldClass ClassPrivate { get; }
    FFieldObjectUnion Owner { get; }
    IFField? Next { get; }
    string NamePrivate { get; }
    EObjectFlags FlagsPrivate { get; }
}