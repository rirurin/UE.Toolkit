namespace UE.Toolkit.Reloaded.Common;

public static unsafe class ToolkitUtils
{
    public static nint GetGlobalAddress(nint address) => *(int*)address + address + 4;
}