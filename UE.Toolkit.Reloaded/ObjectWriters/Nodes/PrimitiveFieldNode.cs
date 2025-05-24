using System.Xml;
using UE.Toolkit.Core.Types.Interfaces;
using UE.Toolkit.Reloaded.ObjectWriters.Writers;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public class PrimitiveFieldNode(string fieldName, nint fieldPtr, Type fieldType, IObjectCreator objCreator, string? baseObjsDir = null) : IFieldNode
{
    private readonly IFieldWriter? _writer = FieldWriterFactory.Create(fieldName, fieldPtr, fieldType, objCreator);
        
    public void ConsumeNode(XmlReader reader)
    {
        var fieldValue = GetFieldValue(reader);
        _writer?.SetField(fieldValue);
        
        reader.Read(); // Consume node.
        Log.Verbose($"{nameof(PrimitiveFieldNode)} || Field '{fieldName}' node consumed.");
    }

    private string GetFieldValue(XmlReader reader)
    {
        // Get content from external text file.
        // Attribute path is relative to the objects' folder.
        if (baseObjsDir != null && reader.GetAttribute("ext-file") is { } extFileRelative)
        {
            var extFile = Path.Join(baseObjsDir, extFileRelative);
            return File.ReadAllText(extFile);
        }

        return reader.GetAttribute("value") ?? reader.ReadElementContentAsString();
    }
}