using System.Xml;
using UE.Toolkit.Reloaded.ObjectWriters.Nodes;

namespace UE.Toolkit.Reloaded.ObjectWriters;

public class ObjectWriter(string objName, Type objType, string objFile, FieldNodeFactory nodeFactory)
{
    private readonly byte[] _xmlContent = File.ReadAllBytes(objFile);

    public string ObjectName { get; } = objName;

    public void WriteToObject(nint objPtr)
    {
        using var reader = XmlReader.Create(new MemoryStream(_xmlContent));
        reader.MoveToContent();
        
        var rootStruct = nodeFactory.Create(ObjectName, objPtr, objType);
        rootStruct.ConsumeNode(reader);
    }
}
