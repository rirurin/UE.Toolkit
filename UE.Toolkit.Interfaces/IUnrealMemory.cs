namespace UE.Toolkit.Interfaces;

public interface IUnrealMemory
{
    nint FMemoryMalloc(nint count, int alignment = 0);
    void FMemoryFree(nint original);
}