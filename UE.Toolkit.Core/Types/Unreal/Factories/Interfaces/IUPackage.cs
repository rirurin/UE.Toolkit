using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUPackage : IUObject
{
    public ulong PackageId { get; }
    public ulong FileSize { get; }
    public FName FileName { get; }
}