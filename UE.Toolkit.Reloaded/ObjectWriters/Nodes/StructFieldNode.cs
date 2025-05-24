using System.Runtime.InteropServices;
using System.Xml;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class StructFieldNode : IFieldNode
{
    private static readonly Dictionary<string, Dictionary<string, Type>> CachedStructsFields = [];
    
    private readonly Dictionary<string, Type> _fields;
    private readonly string _structName;
    private readonly nint _structPtr;
    private readonly Type _structType;
    private readonly FieldNodeFactory _nodeFactory;

    public StructFieldNode(string structName, nint structPtr, Type structType, FieldNodeFactory nodeFactory)
    {
        _structName = structName;
        _structPtr = structPtr;
        _structType = structType;
        _nodeFactory = nodeFactory;

        // Cache struct fields data cause reflection slow...
        if (CachedStructsFields.TryGetValue(structType.Name, out var cachedFields))
        {
            _fields = cachedFields;
        }
        else
        {
            _fields = structType.GetFields().ToDictionary(x => x.Name, x => x.FieldType);
            CachedStructsFields[structType.Name] = _fields;
        }
    }

    public void ConsumeNode(XmlReader reader)
    {
        var anyElementFound = false;
        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element) continue;
            if (AtItemElement(reader)) Log.Warning($"{nameof(StructFieldNode)} || Unexpected '{WriterConstants.ItemTag}' element found. Error?");
            
            anyElementFound = true;
            
            var fieldName = reader.Name;
            if (_fields.TryGetValue(fieldName, out var fieldType))
            {
                var fieldPtr = _structPtr + Marshal.OffsetOf(_structType, fieldName);
                if (_nodeFactory.TryCreate(fieldName, fieldPtr, fieldType, out var fieldNode))
                {
                    fieldNode.ConsumeNode(reader);
                }
                else
                {
                    Log.Warning($"{nameof(StructFieldNode)} || Unsupported node type: {fieldType.Name}");
                    break;
                }
            }
            else
            {
                Log.Warning($"{nameof(StructFieldNode)} || Field '{fieldName}' not found in struct '{_structType.Name}'.");
                break;
            }
        }
        
        if (!anyElementFound) Log.Warning($"{nameof(StructFieldNode)} || No elements found in '{_structName}'. Error?");
        Log.Verbose($"{nameof(StructFieldNode)} || Field '{_structName}' node consumed.");
    }

    private static bool AtItemElement(XmlReader reader)
        => reader.Name == WriterConstants.ItemTag
           && reader.GetAttribute(WriterConstants.ItemIdAttr) != null;
}