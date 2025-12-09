using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Interfaces;

public interface IInvocationParameter
{
    object? Value { get; }
    
    void ToAlloc(nint pAlloc); // for in parameters
    void FromAlloc(nint pAlloc); // for out parameters/return type
    
    string ExpectedPropertyClass { get; }
    int ExpectedSize { get; }
}

public abstract class InvocationParameterCopyable<T>(T value) : IInvocationParameter where T: unmanaged
{
    protected T InnerValue { get; set; } = value;
    public object? Value => InnerValue;
    
    public unsafe void ToAlloc(nint pAlloc) => *(T*)pAlloc = InnerValue;
    public unsafe void FromAlloc(nint pAlloc) => InnerValue = *(T*)pAlloc;
    public int ExpectedSize => Marshal.SizeOf<T>();
    public abstract string ExpectedPropertyClass { get; }
}