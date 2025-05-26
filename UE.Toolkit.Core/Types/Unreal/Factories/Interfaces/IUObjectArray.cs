// ReSharper disable InconsistentNaming
namespace UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;

public interface IUObjectArray
{
    int NumElements { get; }
    
    int ObjLastNonGCIndex { get; }
    
    IUObject? IndexToObject(int idx);

}