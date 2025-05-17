using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Types.Wrappers;

public unsafe class UObjectWrapper<TObject>(TObject* instance)
    where TObject : unmanaged
{
    public TObject* Instance { get; } = instance;

    public string Name { get; } = ((UObjectBase*)instance)->NamePrivate.ToString();

    public string ClassName { get; } = ToolkitUtils.GetUObjectName(((UObjectBase*)instance)->ClassPrivate);
}