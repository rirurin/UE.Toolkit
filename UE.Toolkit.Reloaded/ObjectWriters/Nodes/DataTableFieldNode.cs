using System.Diagnostics.CodeAnalysis;
using System.Xml;
using UE.Toolkit.Core.Types.Unreal.UE5_4_4;
using UE.Toolkit.Core.Types.Wrappers;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class DataTableFieldNode(string fieldName, nint fieldPtr, Type fieldType, FieldNodeFactory nodeFactory) : IFieldNode
{
    public unsafe void ConsumeNode(XmlReader reader)
    {
        // DataTable item type.
        if (!TryGetRowType(reader, out var itemType)) return;
        
        // DataTable map type is always FName + Pointer, can just use UObject.
        var tempTable = new UDataTableWrapper<UObjectBase>((UDataTable<UObjectBase>*)fieldPtr); 
        
        // Get any item nodes.
        using var subReader = reader.ReadSubtree();
        subReader.MoveToContent();
        
        while (subReader.Read())
        {
            if (subReader.NodeType != XmlNodeType.Element) continue;
            if (subReader.Name != WriterConstants.ItemTag) throw new($"Only '{WriterConstants.ItemTag}' elements can be directly inside a UDataTable. Found: {subReader.Name}");
            
            var id = subReader.GetAttribute(WriterConstants.ItemIdAttr);
            if (id == null)
            {
                Log.Error($"{nameof(DataTableFieldNode)} || '{WriterConstants.ItemTag}' in field '{fieldName}' is missing an ID.");
                break;
            }

            var row = tempTable.FirstOrDefault(x => x.Name == id);
            if (row == null)
            {
                Log.Error($"{nameof(DataTableFieldNode)} || Failed to find row with ID '{id}' in '{fieldName}'.");;
                break;
            }

            var rowValuePtr = (nint)row.Instance->Value;
            if (nodeFactory.TryCreate($"{fieldName} (ID: {id})", rowValuePtr, itemType, out var itemNode))
            {
                var itemTree = subReader.ReadSubtree();
                itemTree.MoveToContent();
                
                itemNode.ConsumeNode(itemTree);
            }
        }
        
        Log.Verbose($"{nameof(DataTableFieldNode)} || Field '{fieldName}' node consumed.");
    }

    private bool TryGetRowType(XmlReader reader, [NotNullWhen(true)] out Type? rowType)
    {
        // DataTable is the property of an object with generic type.
        if (fieldType.IsGenericType)
        {
            rowType = fieldType.GetGenericArguments().FirstOrDefault();
        }
        
        // DataTable root object, type needs to be specified as attribute.
        else
        {
            var rowStruct = reader.GetAttribute(WriterConstants.RowStructAttr);
            if (rowStruct == null)
            {
                rowType = null;
                Log.Error($"{nameof(DataTableFieldNode)} || Root '{nameof(UDataTable<byte>)}' missing '{WriterConstants.RowStructAttr}' attribute.");
                return false;
            }

            var rowStructHint = reader.GetAttribute(WriterConstants.RowStructHintAttr);
            var rowStructProvider = reader.GetAttribute(WriterConstants.RowStructProviderAttr);
            nodeFactory.TypeRegistry.TryGetType(rowStruct, rowStructHint, rowStructProvider, out rowType);
        }

        return rowType != null;
    }
}