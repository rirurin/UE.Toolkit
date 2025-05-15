using System.Collections;
using UE.Toolkit.Interfaces.Common.Types.Unreal;

namespace UE.Toolkit.Interfaces.Common.Types.DataTables;

/// <summary>
/// <see cref="UDataTable{TRow}"/> wrapper for simpler use.
/// </summary>
/// <param name="table">The <see cref="UDataTable{TRow}"/> instance.</param>
/// <typeparam name="TRow">Row type.</typeparam>
public unsafe class DataTable<TRow>(UDataTable<TRow>* table)
    : IEnumerator<DataTableRow<TRow>>, IEnumerable<DataTableRow<TRow>>
    where TRow : unmanaged
{
    private int _position;

    public UDataTable<TRow>* Instance { get; } = table;

    public string Name { get; } = table->BaseObj.NamePrivate.ToString()!;

    public string RowStructName => Instance->RowStruct->Super.Super.Obj.NamePrivate.ToString()!;

    #region INTERFACES
    public int Count => Instance->RowMap.MapNum;
    
    public DataTableRow<TRow> this[int index] => new(&Instance->RowMap.Elements[index]);

    public bool MoveNext() => ++_position < Instance->RowMap.MapNum;

    public void Reset() => _position = -1;
    
    object IEnumerator.Current => Current;
    
    public DataTableRow<TRow> Current => new(&Instance->RowMap.Elements[_position]);

    public IEnumerator<DataTableRow<TRow>> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public void Dispose() { }
    #endregion
}