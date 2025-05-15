// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using System.Text;
using UE.Toolkit.Interfaces;
using UE.Toolkit.Interfaces.Common.Types.Unreal;

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealObjects : IUnrealObjects
{
    private delegate FText* FText_FromString(FText* ptr, FString* str);
    private readonly SHFunction<FText_FromString>? _FText_FromString = new("48 89 5C 24 ?? 57 48 83 EC 40 83 7A ?? 01 48 8B D9 48 89 74 24 ?? 4C 89 74 24 ?? C7 44 24 ?? 00 00 00 00 7F ?? E8 ?? ?? ?? ?? 48 8B F0 48 8B 38 48 89 7C 24 ?? 48 85 FF 74 ?? 48 8B 17 48 8B CF FF 52 ?? 8B 46 ?? 4C 8D 74 24 ?? 89 44 24 ?? BE 01 00 00 00 48 8B CF EB ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 8B 7C 24 ?? 4C 8B F0 BE 02 00 00 00 48 8B 08 48 89 0B 48 85 C9 74 ?? 48 8B 11 FF 52 ?? 41 8B 46 ?? 4C 8B 74 24 ?? 89 43 ?? 40 F6 C6 02 74 ?? 48 8B 4C 24 ?? 83 E6 FD 48 85 C9 74 ?? 48 8B 01 FF 50 ?? 40 F6 C6 01 48 8B 74 24 ?? 74 ?? 48 85 FF 74 ?? 48 8B 07 48 8B CF FF 50 ?? 83 4B ?? 12");

    public FText* CreateFText(string str)
    {
        var fstring = CreateFString(str);
        var ftext = (FText*)UnrealMemory._FMemory_Malloc!.Wrapper(sizeof(FText));
        _FText_FromString!.Wrapper(ftext, fstring);
        
        UnrealMemory._FMemory_Free!.Wrapper((nint)fstring);
        return ftext;
    }

    public FString* CreateFString(string str)
    {
        var strBytes = Encoding.Unicode.GetBytes(str);
        var fstring = (FString*)UnrealMemory._FMemory_Malloc!.Wrapper(strBytes.Length);
        
        fstring->Data.ArrayNum = str.Length;
        fstring->Data.ArrayMax = str.Length;
        fstring->Data.AllocatorInstance = (char*)UnrealMemory._FMemory_Malloc.Wrapper(strBytes.Length);
        Marshal.Copy(strBytes, 0, (nint)fstring->Data.AllocatorInstance, strBytes.Length);
        
        return fstring;
    }
}