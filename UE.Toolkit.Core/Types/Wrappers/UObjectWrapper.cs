using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Core.Types.Wrappers;

public unsafe class UObjectWrapper<TObject>(TObject* instance)
    where TObject : unmanaged
{
    public TObject* Instance { get; } = instance;

    public string Name => ToolkitUtils.GetNativeName((nint)Instance);

    public string ClassName => ToolkitUtils.GetNativeName((nint)((UObjectBase*)Instance)->ClassPrivate);
}