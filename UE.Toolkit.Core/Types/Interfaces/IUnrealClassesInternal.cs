using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Interfaces;

public class FieldClassGlobal(nint vtable, IFFieldClass param)
{
    public readonly nint Vtable = vtable;
    public readonly IFFieldClass Params = param;
}

public interface IUnrealClassesInternal
{
    public bool GetFieldClassGlobal(FName Name, out FieldClassGlobal FieldClass);
}