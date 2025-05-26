using UE.Toolkit.Interfaces;
using UE.Toolkit.Reloaded.ObjectWriters;

namespace UE.Toolkit.Reloaded.Toolkit;

public class ToolkitApi(ObjectWriterService objWriters) : IToolkit
{
    public void AddToolkitFolder(string folder)
    {
        var objsDir = Path.Join(folder, "objects");
        if (Directory.Exists(objsDir)) objWriters.AddPath(objsDir);
    }

    public void AddObjectsPath(string path) => objWriters.AddPath(path);
}