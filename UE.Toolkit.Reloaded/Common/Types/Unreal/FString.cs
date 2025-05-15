using System.Runtime.InteropServices;

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FString
{
    public TArray<char> Data;
}