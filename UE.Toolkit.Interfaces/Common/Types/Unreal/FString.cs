using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FString
{
    public TArray<char> Data;

    public override string ToString() => new(Data.AllocatorInstance, 0, Data.ArrayNum);
}