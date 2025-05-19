namespace UE.Toolkit.Core.Types;

public readonly unsafe struct Ptr<T>(T* value)
    where T : unmanaged
{
    public readonly T* Value = value;
}