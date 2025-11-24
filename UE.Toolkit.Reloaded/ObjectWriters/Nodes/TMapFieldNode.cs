using System.Runtime.InteropServices;
using System.Xml;
using UE.Toolkit.Core.Types;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class TMapIntFieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory) : IFieldNode
{
    public unsafe void ConsumeNode(XmlReader reader)
    {
        // TMap<int, ...> item type
        var valueType = fieldType.GetGenericArguments()[1];
        var valueSize = Marshal.SizeOf(valueType);
        
        // TMap values are always by-reference.
        var tempMap = new TMapDynamicDictionary<HashableInt8>((TMap<HashableInt8, byte>*)fieldPtr, valueType, nodeFactory.Memory);
        
        // Get any item nodes.
        using var subReader = reader.ReadSubtree();
        subReader.MoveToContent();

        while (subReader.Read())
        {
            if (subReader.NodeType != XmlNodeType.Element) continue;
            if (subReader.Name != WriterConstants.ItemTag) throw new($"Only '{WriterConstants.ItemTag}' elements can be directly inside a map. Found: {subReader.Name}");
            
            var id = subReader.GetAttribute(WriterConstants.ItemIdAttr);
            if (id == null)
            {
                Log.Error($"{nameof(TArrayFieldNode)} || '{WriterConstants.ItemTag}' is missing an ID.");
                break;
            }
            
            if (!int.TryParse(id, out var itemIdx))
            {
                Log.Warning($"{nameof(TMapIntFieldNode)} || Invalid ID: {id}");
                break;
            }

            // Get existing value
            
            /*
            if (!tempMap.ContainsKey(new HashableInt8(itemIdx)))
            {
                var newEntry = new Ptr<nint>((nint*)nodeFactory.Memory.MallocZeroed(valueSize));
                tempMap.Add(new HashableInt8(itemIdx), newEntry);
                Log.Debug($"{nameof(TMapIntFieldNode)} || Added entry at 0x{(nint)newEntry.Value:x} with key '{itemIdx}' into '{fieldName}'");   
            }
            */

            if (tempMap.TryGetValue(new HashableInt8(itemIdx), out var valuePtr))
            {
                if (nodeFactory.TryCreate($"{fieldName} (Key: {itemIdx})", valuePtr, valueType, out var itemNode))
                {
                    var itemTree = subReader.ReadSubtree();
                    itemTree.MoveToContent();
            
                    itemNode.ConsumeNode(itemTree);
                }   
            }
        }
        
        Log.Verbose($"{nameof(TMapIntFieldNode)} || Field '{fieldName}' node consumed.");
    }
}