using UE.Toolkit.Core.Types.Wrappers;

namespace UE.Toolkit.Interfaces;

public interface IDataTables
{
    void OnDataTableChanged<TRow>(string name, Action<UDataTableWrapper<TRow>> callback)
        where TRow : unmanaged;
}