// ReSharper disable InconsistentNaming

using System.Collections;
using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMap<InKeyType, InValueType>
    where InKeyType : unmanaged
    where InValueType : unmanaged
{
    // From TArray
    public TMapElement<InKeyType, InValueType>* Elements; // Data
    public int MapNum; // ArrayNum
    public int MapMax; // ArrayMax

    // From TSparseArray
    // public TBitArray AllocationFlags;
    // public int FirstFreeIndex;
    // public int NumFreeIndices;
    
    // From TSet (probably)
}

public unsafe class TMapDictionary<InKeyType, InValueType> : IDictionary<InKeyType, Ptr<InValueType>>, IEnumerator<KeyValuePair<InKeyType, Ptr<InValueType>>>
    where InKeyType : unmanaged
    where InValueType : unmanaged
{
    
    private readonly TMap<InKeyType, InValueType>* _map;

    public TMapDictionary(TMap<InKeyType, InValueType>* map)
    {
        _map = map;

        var keys = new List<InKeyType>();
        var values = new List<Ptr<InValueType>>();
        
        for (int i = 0; i < map->MapNum; i++)
        {
            var item = &map->Elements[i];
            var key = item->Key;
            keys.Add(key);
        }
    }
    
    #region DICTIONARY

    public IEnumerator<KeyValuePair<InKeyType, Ptr<InValueType>>> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(KeyValuePair<InKeyType, Ptr<InValueType>> item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(KeyValuePair<InKeyType, Ptr<InValueType>> item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<InKeyType, Ptr<InValueType>>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<InKeyType, Ptr<InValueType>> item)
    {
        throw new NotImplementedException();
    }

    public int Count => _map->MapNum;
    
    public bool IsReadOnly { get; }
    
    public void Add(InKeyType key, Ptr<InValueType> value)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(InKeyType key) => this.Any(x => x.Key.Equals(key));

    public bool Remove(InKeyType key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(InKeyType key, out Ptr<InValueType> value)
    {
        if (ContainsKey(key))
        {
            value = this.First(x => x.Key.Equals(key)).Value;
            return true;
        }

        value = default;
        return false;
    }

    public Ptr<InValueType> this[InKeyType key]
    {
        get
        {
            if (ContainsKey(key)) return this.First(x => x.Key.Equals(key)).Value;
            throw new KeyNotFoundException();
        }

        set
        {
            for (var i = 0; i < _map->MapNum; i++)
            {
                var currItem = &_map->Elements[i];
                if (!currItem->Key.Equals(key)) continue;
                
                currItem->Value = value.Value;
                return;
            }
            
            throw new KeyNotFoundException();
        }
    }

    public ICollection<InKeyType> Keys => this.Select(x => x.Key).ToArray();

    public ICollection<Ptr<InValueType>> Values => this.Select(x => x.Value).ToArray();

    #endregion

    #region ENUMERATOR

    private int _position = -1;

    public bool MoveNext()
    {
        if (_map == null) return false;
        if (_map->Elements == null) return false;
        return ++_position < _map->MapNum;
    }

    public void Reset() => _position = -1;

    public KeyValuePair<InKeyType, Ptr<InValueType>> Current
    {
        get
        {
            var curr = _map->Elements[_position];
            return new(curr.Key, new(curr.Value));
        }
    }

    object? IEnumerator.Current => Current;

    public void Dispose() { }

    #endregion
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct TMapElement<InKeyType, InValueType>
    where InKeyType : unmanaged
    where InValueType : unmanaged
{
    public InKeyType Key;
    public InValueType* Value;
    
    // From TSetElementBase
    /** The id of the next element in the same hash bucket. */
    public int HashNextId; // FSetElementId

    /** The hash bucket that the element is currently linked to. */
    public int HashIndex;
}

