using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct UUserDefinedEnum
{
    public UEnum Super;
    public TMap<FName, FText> DisplayNameMap;
}