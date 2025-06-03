using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UE.Toolkit.Core.Types.Interfaces;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential, Size = 0xB0)]
public unsafe struct UDataTable<TRow>
    where TRow : unmanaged
{
    public UObjectBase BaseObj;
    public UScriptStruct* RowStruct;
    public TMap<FName, TRow> RowMap;
}

public class UDataTableManaged<TRow> : IDictionary<FName, Ptr<TRow>>, IDisposable
    where TRow : unmanaged
{
    public unsafe UDataTable<TRow>* Self { get; private set; }

    protected IUnrealMemoryInternal Allocator;
    protected TMapDictionary<FName, TRow> RowMap;
    protected bool OwnsInstance;
    protected bool Disposed = false;

    public unsafe UDataTableManaged(UDataTable<TRow>* _Self, IUnrealMemoryInternal _Allocator)
    {
        Self = _Self;
        Allocator = _Allocator;
        RowMap = new(&Self->RowMap, Allocator);
        OwnsInstance = false;
    }

    public unsafe string Name
    { 
        get => Self->BaseObj.NamePrivate.ToString();
    }

    public unsafe string RowStructName => Self->RowStruct->Super.Super.Super.NamePrivate.ToString();

    public unsafe void AddRow(FName RowName, TRow RowDataPtr) => Add(RowName, new(&RowDataPtr));

    public ICollection<FName> GetRowNames() => Keys;

    #region DICTIONARY INTERFACE
    public ICollection<FName> Keys => RowMap.Keys;

    public ICollection<Ptr<TRow>> Values => RowMap.Values;

    public int Count => RowMap.Count;

    public bool IsReadOnly => false;

    public Ptr<TRow> this[FName key] 
    {
        get => RowMap[key];
        set => RowMap[key] = value;
    }

    public void Add(FName key, Ptr<TRow> value) => RowMap.Add(key, value);

    public bool ContainsKey(FName key) => RowMap.ContainsKey(key);

    public bool Remove(FName key) => RowMap.Remove(key);

    public bool TryGetValue(FName key, [MaybeNullWhen(false)] out Ptr<TRow> value) => RowMap.TryGetValue(key, out value);

    public void Add(KeyValuePair<FName, Ptr<TRow>> item) => RowMap.Add(item);

    public void Clear() => RowMap.Clear();

    public bool Contains(KeyValuePair<FName, Ptr<TRow>> item) => RowMap.Contains(item);

    public void CopyTo(KeyValuePair<FName, Ptr<TRow>>[] array, int arrayIndex) => RowMap.CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<FName, Ptr<TRow>> item) => RowMap.Remove(item);

    public IEnumerator<KeyValuePair<FName, Ptr<TRow>>> GetEnumerator() => RowMap.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region DISPOSE INTERFACE

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing) // Dispose managed resources
            {
                RowMap.Dispose();
            }
            // Disposed unmanaged resources (for Unreal)
            if (OwnsInstance)
            {
                unsafe
                {
                    Allocator.Free((nint)Self);
                }
            }
            Disposed = true;
        }
    }

    ~UDataTableManaged() => Dispose(false);

    #endregion
}