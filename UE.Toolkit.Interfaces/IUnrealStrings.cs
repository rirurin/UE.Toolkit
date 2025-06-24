using UE.Toolkit.Core.Types.Unreal.UE5_4_4;

namespace UE.Toolkit.Interfaces;

/// <summary>
/// Unreal strings API.
/// </summary>
public unsafe interface IUnrealStrings
{
    /// <summary>
    /// Gets the string representation of a <see cref="FText"/>.
    /// </summary>
    /// <param name="text"><see cref="FText"/> instance..</param>
    /// <returns><see cref="FText"/> value as a string.</returns>
    /// <remarks>May sometimes return <c>MISSING STRING TABLE ENTRY</c>.</remarks>
    string FTextToString(FText* text);
    
    /// <summary>
    /// Creates a new <see cref="FString"/> with the given content, if any.
    /// </summary>
    /// <param name="content">String content.</param>
    /// <returns><see cref="FString"/> instance.</returns>
    FString* CreateFString(string? content = null);
    
    /// <summary>
    /// Try to get the table ID and string key of a <see cref="FText"/>.
    /// </summary>
    /// <param name="text"><see cref="FText"/> instance.</param>
    /// <param name="tableId">Table ID.</param>
    /// <param name="key">String key.</param>
    /// <returns>True if successful, false otherwise.</returns>
    bool FTextInspectorGetTableIdAndKey(FText* text, out string? tableId, out string? key);
    
    /// <summary>
    /// Gets the display name for the member of a UUserDefinedEnum.
    /// </summary>
    /// <param name="userEnum">UUserDefinedEnum pointer.</param>
    /// <param name="index">Member index to get name of.</param>
    /// <returns>The enum member's name.</returns>
    string UEnumGetDisplayNameTextByIndex(nint userEnum, int index);
}