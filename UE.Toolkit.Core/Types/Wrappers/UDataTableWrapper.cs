using System.Collections;
using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Types.Wrappers;

/// <summary>
/// <see cref="UDataTable{TRow}"/> wrapper for simpler use.
/// </summary>
/// <param name="table">The <see cref="UDataTable{TRow}"/> instance.</param>
/// <typeparam name="TRow">Row type.</typeparam>
public unsafe class UDataTableWrapper<TRow>(UDataTable<TRow>* table)
    : IEnumerable<DataTableRow<TRow>>
    where TRow : unmanaged
{
    public UDataTable<TRow>* Instance { get; } = table;

    public string Name { get; } = table->BaseObj.NamePrivate.ToString();

    public string RowStructName => Instance->RowStruct->Super.Super.Super.NamePrivate.ToString();

    #region INTERFACES
    public int Count => Instance->RowMap.MapNum;
    
    public DataTableRow<TRow> this[int index] => new(&Instance->RowMap.Elements[index]);

    public IEnumerator<DataTableRow<TRow>> GetEnumerator() => new UDataTableWrapperEnumerator<TRow>(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public void Dispose() { }
    #endregion
}

public unsafe class UDataTableWrapperEnumerator<TRow>(UDataTableWrapper<TRow> table) : IEnumerator<DataTableRow<TRow>>
    where TRow : unmanaged
{
    private int _position;
    
    public bool MoveNext() => ++_position < table.Instance->RowMap.MapNum;

    public void Reset() => _position = -1;

    public DataTableRow<TRow> Current => new(&table.Instance->RowMap.Elements[_position]);

    object? IEnumerator.Current => Current;

    public void Dispose() { }
}

/// <summary>
/// <see cref="UDataTableWrapper{TRow}"/> row wrapper for simpler use.
/// </summary>
/// <param name="row">Row instance.</param>
/// <typeparam name="TRow">Row type.</typeparam>
public unsafe class DataTableRow<TRow>(TMapElement<FName, TRow>* row)
    where TRow : unmanaged
{
    public TMapElement<FName, TRow>* Instance { get; } = row;

    public string Name { get; } = row->Key.ToString();
}