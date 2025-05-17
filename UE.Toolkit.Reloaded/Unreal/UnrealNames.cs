using System.Runtime.InteropServices;
using UE.Toolkit.Interfaces.Common;
using UE.Toolkit.Interfaces.Common.Types.Unreal;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealNames
{
    private delegate FName FNameHelper_MakeWithNumber(FName* fname, FWideStringViewWithWidth* view, EFindName findType, int internalNumber);
    private readonly SHFunction<FNameHelper_MakeWithNumber>? _FNameHelper_MakeWithNumber;
    
    public UnrealNames()
    {
        ScanHooks.Add(nameof(FName.GFNamePool), "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 4C 8B C0 C6 05 ?? ?? ?? ?? 01 8B D3 0F B7 C3 89 44 24", (_, result) => FName.GFNamePool = (FNamePool*)ToolkitUtils.GetGlobalAddress(result + 3));
        
        // TODO: Should be made with a function pointer for perf.
        //_FNameHelper_MakeWithNumber = new(FNameHelper_MakeWithNumberImpl, "40 55 53 57 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 70 04 00 00");
    }
    
    private FName FNameHelper_MakeWithNumberImpl(FName* fname, FWideStringViewWithWidth* view, EFindName findType, int internalNumber)
    {
        var str = new string(view->WideStr, 0, view->Len);
        Log.Information(str);
        
        return _FNameHelper_MakeWithNumber!.Hook!.OriginalFunction(fname, view, findType, internalNumber);
    }
    
    private enum EFindName
    {
        FNAME_Find,
        FNAME_Add
    }
    
    [StructLayout(LayoutKind.Explicit)]
    private struct FWideStringViewWithWidth
    {
        [FieldOffset(0x0)] public char* WideStr;
        [FieldOffset(0x8)] public int Len;
        [FieldOffset(0xC)] public bool bIsWide;
    }
}