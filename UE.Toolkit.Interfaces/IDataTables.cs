using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Core.Types.Wrappers;

namespace UE.Toolkit.Interfaces;

/// <summary>
/// DataTables service.
/// </summary>
public interface IDataTables
{
    /// <summary>
    /// Notify whenever a <see cref="UDataTable{TRow}"/> is changed, typically just when loaded, and receive it in simple wrapper.
    /// </summary>
    /// <param name="name"><see cref="UDataTable{TRow}"/> object name.</param>
    /// <param name="callback">Callback function receiving the <see cref="UDataTable{TRow}"/>.</param>
    /// <typeparam name="TRow"><see cref="UDataTable{TRow}"/> row struct type. Use <see cref="UObjectBase"/> to handle typing yourself.</typeparam>
    void OnDataTableChanged<TRow>(string name, Action<UDataTableWrapper<TRow>> callback)
        where TRow : unmanaged;
}