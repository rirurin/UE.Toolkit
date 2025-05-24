namespace UE.Toolkit.Interfaces;

/// <summary>
/// Toolkit API interface.
/// </summary>
public interface IToolkit
{
    /// <summary>
    /// Add a path to load object XML file(s) from.
    /// </summary>
    /// <param name="path">File or folder path.</param>
    void AddObjectsPath(string path);
}