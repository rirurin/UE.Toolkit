using System.Xml;

namespace UE.Toolkit.Reloaded.ObjectWriters.Nodes;

public interface IFieldNode
{
    void ConsumeNode(XmlReader reader);
}