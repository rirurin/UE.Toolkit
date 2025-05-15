using UE.Toolkit.Reloaded.Common.Types.Unreal;

namespace UE.Toolkit.Reloaded.Common.Types.DataTables;

/// <summary>
/// <see cref="DataTable{TRow}"/> row wrapper for simpler use.
/// </summary>
/// <param name="row"></param>
/// <typeparam name="TRow"></typeparam>
public unsafe class DataTableRow<TRow>(TMapElement<FName, TRow>* row)
    where TRow : unmanaged
{
    public TMapElement<FName, TRow>* Instance { get; } = row;

    public string Name { get; } = row->Key.ToString()!;

    public TRow* Value => Instance->Value;
}