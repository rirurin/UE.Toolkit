using System.Runtime.InteropServices;

// ReSharper disable CommentTypo

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct FName
{
    public FNameEntryId ComparisonIndex;
    public int Number; // #if !UE_FNAME_OUTLINE_NUMBER
}

public struct FNameEntryId
{
    public int Value;
}