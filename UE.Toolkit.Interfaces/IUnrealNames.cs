namespace UE.Toolkit.Interfaces;

/// <summary>
/// Unreal Names API.
/// </summary>
public interface IUnrealNames
{
    /// <summary>
    /// Redirects the construction/retrieval of an FName to a new value.
    /// </summary>
    /// <param name="modName">Name of mod editing FName.</param>
    /// <param name="fname">FName to redirect.</param>
    /// <param name="newValue">Value to redirect FName to.</param>
    /// <remarks>
    /// Very hacky method for editing string values in objects that would be difficult to edit otherwise.<br/>
    /// Current implementation only hooks the FName Unicode constructor; FNames created with other methods may be missed.
    /// </remarks>
    void RedirectFName(string modName, string fname, string newValue);
}