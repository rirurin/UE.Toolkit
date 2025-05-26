namespace UE.Toolkit.Interfaces;

/// <summary>
/// Unreal memory API.
/// </summary>
public interface IUnrealMemory
{
    /// <summary>
    /// Allocates a block of memory with the global <c>FMemory</c>.
    /// </summary>
    /// <param name="count">Number of bytes to allocate.</param>
    /// <param name="alignment">Alignment of allocation.</param>
    /// <returns>Pointer to the allocated memory.</returns>
    /// <remarks>Should only be used after Unreal has finished loading.</remarks>
    nint Malloc(nint count, int alignment = 0);
    
    /// <summary>
    /// Deallocates a block of memory allocated with the global <c>FMemory</c>.
    /// </summary>
    /// <param name="original">Pointer to the memory block to free.</param>
    /// <remarks>Should only be used after Unreal has finished loading.</remarks>
    void Free(nint original);
}