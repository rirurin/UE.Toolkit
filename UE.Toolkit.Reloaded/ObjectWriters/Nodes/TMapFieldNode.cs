using System.Runtime.InteropServices;
using System.Xml;
using UE.Toolkit.Core.Types;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Interfaces;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public abstract class TMapBaseFieldNode<TKeyValue>(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory)
    : IFieldNode where TKeyValue : unmanaged, IMapHashable, IEquatable<TKeyValue>
{
    public void ConsumeNode(XmlReader reader)
    {
        var valueType = fieldType.GetGenericArguments()[1];
        var valueSize = Marshal.SizeOf(valueType);

        var tempMap = CreateTempMap(fieldPtr, valueType, nodeFactory.Memory);
        
        // Get any item nodes.
        using var subReader = reader.ReadSubtree();
        subReader.MoveToContent();

        while (subReader.Read())
        {
            if (subReader.NodeType != XmlNodeType.Element) continue;
            if (subReader.Name != WriterConstants.ItemTag)
                throw new(
                    $"Only '{WriterConstants.ItemTag}' elements can be directly inside a map. Found: {subReader.Name}");

            var id = subReader.GetAttribute(WriterConstants.ItemIdAttr);
            if (id == null)
            {
                Log.Error($"{nameof(TMapBaseFieldNode<TKeyValue>)} || '{WriterConstants.ItemTag}' is missing an ID.");
                break;
            }

            if (!CreateKeyValue(id, out var KeyMaybe))
            {
                Log.Error($"{nameof(TMapBaseFieldNode<TKeyValue>)} || Could not process map key {id}");
                break;               
            }
            var Key = KeyMaybe!.Value;
            unsafe
            {
                var tempMapBitAlloc = (TArray<byte>*)(tempMap.Self + 0x20);
                // Ensure that TMap is properly initialized for newly allocated items - BitAllocator's capacity should
                // be 0x80 (128) to account for inline bits (equivalent to Reset()).
                if (tempMapBitAlloc->ArrayMax < 128)
                    tempMapBitAlloc->ArrayMax = 128;
            }
            
            // Get existing value
            if (!ContainsKey(tempMap, Key))
            {
                var newEntry = nodeFactory.Memory.MallocZeroed(valueSize);
                tempMap.AddIndirect(Key, newEntry);
                nodeFactory.Memory.Free(newEntry);
                Log.Debug($"{nameof(TMapBaseFieldNode<TKeyValue>)} || Added entry at 0x{newEntry:x} with key '{Key}' into '{fieldName}'");
            }

            if (tempMap.TryGetValue(Key, out var valuePtr) && 
                nodeFactory.TryCreate($"{fieldName} (Key: {Key})", valuePtr, 0, valueType, out var itemNode))
            {
                var itemTree = subReader.ReadSubtree();
                itemTree.MoveToContent();
                itemNode.ConsumeNode(itemTree);
            }
        }
    }

    public abstract TMapDynamicDictionary<TKeyValue> CreateTempMap(
        nint fieldPtr, Type valueType, IUnrealMemoryInternal memory);

    public abstract bool CreateKeyValue(string id, out TKeyValue? Value);

    public virtual bool ContainsKey(TMapDynamicDictionary<TKeyValue> dict, TKeyValue key)
        => dict.ContainsKey(key);
}

public class TMapIntFieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory)
    : TMapBaseFieldNode<HashableInt>(fieldName, fieldPtr, fieldType, nodeFactory)
{
    public override unsafe TMapDynamicDictionary<HashableInt> CreateTempMap(nint fieldPtr, Type valueType, IUnrealMemoryInternal memory)
        => new((TMap<HashableInt, byte>*)fieldPtr, valueType, nodeFactory.Memory);

    public override bool CreateKeyValue(string id, out HashableInt? Value)
    {
        Value = null;
        if (!int.TryParse(id, out var itemIdx))
        {
            Log.Warning($"{nameof(TMapIntFieldNode)} || Invalid ID: {id}");
            return false;
        }
        Value = new HashableInt(itemIdx);
        return true;
    }
}

public class TMapInt8FieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory)
    : TMapBaseFieldNode<HashableInt8>(fieldName, fieldPtr, fieldType, nodeFactory)
{
    public override unsafe TMapDynamicDictionary<HashableInt8> CreateTempMap(nint fieldPtr, Type valueType, IUnrealMemoryInternal memory)
        => new((TMap<HashableInt8, byte>*)fieldPtr, valueType, nodeFactory.Memory);

    public override bool CreateKeyValue(string id, out HashableInt8? Value)
    {
        Value = null;
        if (!int.TryParse(id, out var itemIdx))
        {
            Log.Warning($"{nameof(TMapInt8FieldNode)} || Invalid ID: {id}");
            return false;
        }
        Value = new HashableInt8(itemIdx);
        return true;
    }
}

public class TMapNameFieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory)
    : TMapBaseFieldNode<FName>(fieldName, fieldPtr, fieldType, nodeFactory)
{
    public override unsafe TMapDynamicDictionary<FName> CreateTempMap(nint fieldPtr, Type valueType, IUnrealMemoryInternal memory)
        => new((TMap<FName, byte>*)fieldPtr, valueType, nodeFactory.Memory);

    public override bool CreateKeyValue(string id, out FName? Value)
    {
        Value = new FName(id);
        return true;
    }
}