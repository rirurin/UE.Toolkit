namespace UE.Toolkit.Interfaces;

public interface IUnrealClasses
{
    /// <summary>
    /// Listen for the creation of an object's class, then extend it's allocation size and call a custom constructor.
    /// </summary>
    /// <param name="ExtraSize">Number of bytes to increase the allocation size of object instances by.</param>
    /// <param name="callback">A custom constructor to call after the original constructor</param>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <remarks>
    /// The extension is only created for the specific object type. If a struct/class inherits from this class, it
    /// won't receive an extension.
    /// </remarks>
    public void AddExtension<TObject>(uint ExtraSize, Action<ToolkitUObject<TObject>> callback) 
        where TObject: unmanaged;
}