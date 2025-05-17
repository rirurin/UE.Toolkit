using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct FFieldClass
{
    public FName Name;
    public ulong Id;
    public ulong CastFlags;
    public EClassFlags ClassFlags;
    public FFieldClass* SuperClass;
    public FField* DefaultObject;
    public nint FieldConstructor;
    public FThreadSafeCounter UnqiueNameIndexCounter;
}

[StructLayout(LayoutKind.Sequential)]
public struct FThreadSafeCounter
{
    public int Counter;
}