namespace UE.Toolkit.Interfaces.Common;

public static unsafe class ToolkitUtils
{
    public static nint GetGlobalAddress(nint address) => *(int*)address + address + 4;
}