namespace UE.Toolkit.Interfaces;

/// <summary>
/// Toolkit API interface.
/// </summary>
public interface IToolkit
{
    /// <summary>
    /// Add a new full UE Toolkit folder, meaning it acts like a mod's <c>ue-toolkit</c>.
    /// </summary>
    /// <param name="folder">Folder path.</param>
    void AddToolkitFolder(string folder);
    
    /// <summary>
    /// Add a path to load object XML file(s) from.
    /// </summary>
    /// <param name="path">File or folder path.</param>
    void AddObjectsPath(string path);
}