using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.ObjectWriters;

namespace UE.Toolkit.Reloaded.Toolkit;

public class ToolkitApi(ObjectWriterService objWriters) : IToolkit
{
    public void AddObjectsPath(string path) => objWriters.AddPath(path);
}