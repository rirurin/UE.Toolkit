using System.Runtime.InteropServices;
using System.Xml;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class TArrayFieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory) : IFieldNode
{
    public unsafe void ConsumeNode(XmlReader reader)
    {
        // TArray item type.
        var itemType = fieldType.GetGenericArguments().First();
        var itemSize = Marshal.SizeOf(itemType);
        var tempArray = (TArray<byte>*)fieldPtr;
        
        // Get any item nodes.
        using var subReader = reader.ReadSubtree();
        subReader.MoveToContent();
        
        while (subReader.Read())
        {
            if (subReader.NodeType != XmlNodeType.Element) continue;
            if (subReader.Name != WriterConstants.ItemTag) throw new($"Only '{WriterConstants.ItemTag}' elements can be directly inside an array. Found: {subReader.Name}");
            
            var id = subReader.GetAttribute(WriterConstants.ItemIdAttr);
            if (id == null)
            {
                Log.Error($"{nameof(TArrayFieldNode)} || '{WriterConstants.ItemTag}' is missing an ID.");
                break;
            }
            
            if (!int.TryParse(id, out var itemIdx))
            {
                Log.Warning($"{nameof(TArrayFieldNode)} || Invalid ID: {id}");
                break;
            }

            // Reserve -1 as a value for pushing a new value into the TArray. Choose a better method perhaps?
            if (itemIdx != -1)
            {
                itemIdx -= 1; // We're doing 1 indexing because normal people can't handle 0...
                if (itemIdx < 0 || itemIdx > tempArray->ArrayNum)
                {
                    Log.Warning($"{nameof(TArrayFieldNode)} || ID is either less than 1 or more than item count: {id} || Total: {tempArray->ArrayNum}");
                    break;
                }   
            } else
            {
                Log.Verbose($"{nameof(TArrayFieldNode)} @ 0x{(nint)tempArray:x} || Old Size : {tempArray->ArrayNum} || Old Capacity: {tempArray->ArrayMax}");
                if (tempArray->ArrayNum == tempArray->ArrayMax)
                {
                    TArrayListStatic.ResizeToStatic(tempArray, 
                        TArrayListStatic.CalculateNewArraySizeStatic(tempArray), itemSize, nodeFactory.Memory);   
                }
                itemIdx = tempArray->ArrayNum;
                tempArray->ArrayNum++;
            }

            var itemPtr = itemIdx * itemSize + (nint)tempArray->AllocatorInstance;
            if (nodeFactory.TryCreate($"{fieldName} (ID: {id})", itemPtr, itemType, out var itemNode))
            {
                itemNode.ConsumeNode(subReader);
            }
        }
        
        Log.Verbose($"{nameof(TArrayFieldNode)} || Field '{fieldName}' node consumed.");
    }
}