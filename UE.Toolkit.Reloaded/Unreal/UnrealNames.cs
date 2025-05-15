// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using UE.Toolkit.Reloaded.Common;
using UE.Toolkit.Reloaded.Common.Types.Unreal;

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealNames
{
    private delegate FName FNameHelper_MakeWithNumber(FName* fname, FWideStringViewWithWidth* view, EFindName findType, int internalNumber);
    private readonly SHFunction<FNameHelper_MakeWithNumber>? _FNameHelper_MakeWithNumber;
    
    public UnrealNames()
    {
        ScanHooks.Add(nameof(GFNamePool), "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 4C 8B C0 C6 05 ?? ?? ?? ?? 01 8B D3 0F B7 C3 89 44 24", (_, result) => GFNamePool = (FNamePool*)ToolkitUtils.GetGlobalAddress(result + 3));
        
        // TODO: Should be made with a function pointer for perf.
        //_FNameHelper_MakeWithNumber = new(FNameHelper_MakeWithNumberImpl, "40 55 53 57 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 70 04 00 00");
    }

    public static FNamePool* GFNamePool { get; private set; } = null;
    
    private FName FNameHelper_MakeWithNumberImpl(FName* fname, FWideStringViewWithWidth* view, EFindName findType, int internalNumber)
    {
        var str = new string(view->WideStr, 0, view->Len);
        Log.Information(str);
        
        return _FNameHelper_MakeWithNumber!.Hook!.OriginalFunction(fname, view, findType, internalNumber);
    }
}

/** Enumeration for finding name. */
public enum EFindName
{
    /**
    * Find a name; return 0/NAME_None/FName() if it doesn't exist.
    * When UE_FNAME_OUTLINE_NUMBER is set, we search for the exact name including the number suffix.
    * Otherwise we search only for the string part.
    */
    FNAME_Find,

    /** Find a name or add it if it doesn't exist. */
    FNAME_Add
};

[StructLayout(LayoutKind.Explicit)]
public unsafe struct FWideStringViewWithWidth
{
    [FieldOffset(0x0)] public char* WideStr;
    [FieldOffset(0x8)] public int Len;
    [FieldOffset(0xC)] public bool bIsWide;
}