using UE.Toolkit.Interfaces.Common.Types.DataTables;

namespace UE.Toolkit.Interfaces;

public interface IDataTables
{
    void OnDataTableChanged<TRow>(string name, Action<DataTable<TRow>> callback)
        where TRow : unmanaged;
}