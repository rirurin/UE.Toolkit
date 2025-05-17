using UE.Toolkit.Interfaces.Common.Types.Unreal;

namespace UE.Toolkit.Interfaces.Common.Types.Wrappers;

public unsafe class UObjectWrapper<TObject>(TObject* instance)
    where TObject : unmanaged
{
    public TObject* Instance { get; } = instance;

    public string Name { get; } = ((UObjectBase*)instance)->NamePrivate.ToString();

    public string ClassName { get; } = ToolkitUtils.GetUObjectName(((UObjectBase*)instance)->ClassPrivate);
}