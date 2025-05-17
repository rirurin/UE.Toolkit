using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct FText
{
    private nint TextData;
    private uint Flags;
}