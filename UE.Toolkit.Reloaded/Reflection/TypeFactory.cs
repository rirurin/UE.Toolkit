using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Reflection;

public abstract class BaseTypeFactory(IUnrealFactory factory, IUnrealMemory memory, 
    IUnrealClasses classes, IPropertyFlagsBuilder flags)
{
    
    // public abstract void CreateScriptStruct(string Name, int Size);
    public abstract bool CreateI8Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateI16Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateI32Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateI64Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateU8Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateU16Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateU32Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateU64Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateF32Param(string Name, int Offset, out IFGenericPropertyParams? Out);
    public abstract bool CreateF64Param(string Name, int Offset, out IFGenericPropertyParams? Out);

    internal abstract bool CreateStructParam(string Name, int Size,
        List<IFPropertyParams> Fields, out IFStructParams? Out);
    
    #region Dependencies
    
    protected readonly IUnrealFactory Factory = factory;
    protected readonly IUnrealMemory Memory = memory;
    protected readonly IUnrealClasses Classes = classes;
    protected readonly IPropertyFlagsBuilder Flags = flags;
    
    #endregion
    
    #region Constants

    protected const int TYPE_ALIGNMENT = 0x10;

    #endregion
}