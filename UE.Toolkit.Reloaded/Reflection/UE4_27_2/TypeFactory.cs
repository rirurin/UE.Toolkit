using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Unreal.Factories;
using UE.Toolkit.Core.Types.Unreal.Factories.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE4_27_2;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.Reflection.UE4_27_2;

public class TypeFactory(IUnrealFactory factory, IUnrealMemory memory, IUnrealClasses classes) 
    : BaseTypeFactory(factory, memory, classes)
{

    private unsafe void SetUObjectFields(IUObject Object, string Name)
    {
        // var pObject = (UObjectBase*)Object.Ptr;
        // pObject->_vtable = 0;
        // pObject->ObjectFlags = EObjectFlags
        // pObject->InternalIndex = 
    }
    
    public override void CreateScriptStruct(string Name, int Size)
    {
        var Alloc = Memory.Malloc(Marshal.SizeOf<UScriptStruct>(), TYPE_ALIGNMENT);
        SetUObjectFields(Factory.CreateUObject(Alloc), Name);
    }
}