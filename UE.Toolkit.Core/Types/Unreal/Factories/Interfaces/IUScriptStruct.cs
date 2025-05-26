// ReSharper disable InconsistentNaming

using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUScriptStruct : IUStruct
{
    EStructFlags StructFlags { get; }
    
    bool bPrepareCppStructOpsCompleted { get; }
    
    nint CppStructOps { get; }
}