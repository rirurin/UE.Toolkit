using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Common;

public static unsafe class ToolkitUtils
{
    public static nint GetGlobalAddress(nint address) => *(int*)address + address + 4;

    /// <summary>
    /// Gets the <see cref="UObjectBase"/> name.
    /// </summary>
    /// <param name="obj">UObject instance.</param>
    /// <returns>The object's name.</returns>
    public static string GetUObjectName<T>(T* obj) where T : unmanaged
        => ((UObjectBase*)obj)->NamePrivate.ToString();
}