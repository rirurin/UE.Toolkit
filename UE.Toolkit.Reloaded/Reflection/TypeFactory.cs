using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Reflection;

public abstract class BaseTypeFactory(IUnrealFactory factory, IUnrealMemory memory, IUnrealClasses classes)
{
    
    public abstract void CreateScriptStruct(string Name, int Size);
    
    #region Dependencies
    
    protected readonly IUnrealFactory Factory = factory;
    protected readonly IUnrealMemory Memory = memory;
    protected readonly IUnrealClasses Classes = classes;   
    
    #endregion
    
    #region Constants

    protected const int TYPE_ALIGNMENT = 0x10;

    #endregion
}