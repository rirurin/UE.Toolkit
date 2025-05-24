using UE.Toolkit.Core.Types.Unreal;

namespace UE.Toolkit.Core.Types.Interfaces;

public unsafe interface IObjectCreator
{
    /// <summary>
    /// Creates an instance of <see cref="FText"/> with the given content.
    /// </summary>
    /// <param name="content">String content.</param>
    /// <returns><see cref="FText"/> instance.</returns>
    FText* CreateFText(string content);
    
    /// <summary>
    /// Creates an instance of <see cref="FString"/> with the given content.
    /// </summary>
    /// <param name="content">String content.</param>
    /// <returns><see cref="FString"/> instance.</returns>
    FString* CreateFString(string content);
}