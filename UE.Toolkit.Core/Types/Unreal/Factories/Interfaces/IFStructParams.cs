using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IFStructParams : IPtr
{
    nint OuterFunc { get; }
    nint SuperFunc { get; }
    nint StructOpsFunc { get; }
    string Name { get; }
    ulong Size { get; }
    ulong Alignment { get; }
    EObjectFlags ObjectFlags { get; }
    int StructFlags { get; }
    int PropertyCount { get; }
    IFPropertyParams? GetProperty(int Index);
    IEnumerable<IFPropertyParams> Properties { get; }
    // IEnumerable<IFPropertyParams> Properties { get; }
}