using System.Runtime.InteropServices;
using System.Text;
using UE.Toolkit.Core.Common;
using UE.Toolkit.Core.Types.Unreal;
using UE.Toolkit.Core.Types.Wrappers;
using UE.Toolkit.Interfaces;

// ReSharper disable InconsistentNaming

namespace UE.Toolkit.Reloaded.Unreal;

public unsafe class UnrealObjects : IUnrealObjects
{
    private delegate FText* FText_FromString(FText* ptr, FString* str);
    private readonly SHFunction<FText_FromString>? _FText_FromString = new("48 89 5C 24 ?? 57 48 83 EC 40 83 7A ?? 01 48 8B D9 48 89 74 24 ?? 4C 89 74 24 ?? C7 44 24 ?? 00 00 00 00 7F ?? E8 ?? ?? ?? ?? 48 8B F0 48 8B 38 48 89 7C 24 ?? 48 85 FF 74 ?? 48 8B 17 48 8B CF FF 52 ?? 8B 46 ?? 4C 8D 74 24 ?? 89 44 24 ?? BE 01 00 00 00 48 8B CF EB ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 8B 7C 24 ?? 4C 8B F0 BE 02 00 00 00 48 8B 08 48 89 0B 48 85 C9 74 ?? 48 8B 11 FF 52 ?? 41 8B 46 ?? 4C 8B 74 24 ?? 89 43 ?? 40 F6 C6 02 74 ?? 48 8B 4C 24 ?? 83 E6 FD 48 85 C9 74 ?? 48 8B 01 FF 50 ?? 40 F6 C6 01 48 8B 74 24 ?? 74 ?? 48 85 FF 74 ?? 48 8B 07 48 8B CF FF 50 ?? 83 4B ?? 12");

    private delegate FString* FText_ToString(FText* text);
    private readonly SHFunction<FText_ToString> _FText_ToString =
        new("40 53 48 83 EC 20 48 8B D9 E8 ?? ?? ?? ?? 48 8B 0B 48 8B 01 48 83 C4 20");
    
    private delegate void UObject_PostLoadSubobjects(UObjectBase* self, FObjectInstancingGraph* outerInstanceGraph);
    private readonly SHFunction<UObject_PostLoadSubobjects> _UObject_PostLoadSubobjects;

    private Action<UObjectWrapper<UObjectBase>>? _onObjectLoaded;

    public UnrealObjects()
    {
        _UObject_PostLoadSubobjects = new(UObject_PostLoadSubobjectsImpl,"40 55 53 41 56 48 8D AC 24 ?? ?? ?? ?? 48 81 EC 10 02 00 00 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 85 ?? ?? ?? ?? 48 8B 41");
        ScanHooks.Add("GUObjectArray", "2B 05 ?? ?? ?? ?? 44 89 05", (_, result) => GUObjectArray = (FUObjectArray*)ToolkitUtils.GetGlobalAddress(result + 2));
    }

    public FUObjectArray* GUObjectArray { get; private set; }

    public void OnObjectLoadedByName<TObject>(string objName, Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged
    {
        _onObjectLoaded += obj =>
        {
            if (obj.Name == objName) callback(new((TObject*)obj.Instance));
        };
    }

    public void OnObjectLoadedByClass<TObject>(string objClass, Action<UObjectWrapper<TObject>> callback)
        where TObject : unmanaged
    {
        _onObjectLoaded += obj =>
        {
            if (obj.Instance->IsChildOf(objClass)) callback(new((TObject*)obj.Instance));
        };
    }
    
    public FText* CreateFText(string content)
    {
        var fstring = CreateFString(content);
        var ftext = (FText*)UnrealMemory._FMemory_Malloc!.Wrapper(sizeof(FText));
        _FText_FromString!.Wrapper(ftext, fstring);
        
        UnrealMemory._FMemory_Free!.Wrapper((nint)fstring);
        return ftext;
    }
    
    public string FTextToString(FText* text) => _FText_ToString.Wrapper(text)->ToString();

    public FString* CreateFString(string content)
    {
        content += '\0';
        
        var fstring = (FString*)UnrealMemory._FMemory_Malloc!.Wrapper(sizeof(FString));
        fstring->Data.ArrayNum = content.Length;
        fstring->Data.ArrayMax = content.Length;
        
        var strBytes = Encoding.Unicode.GetBytes(content);
        fstring->Data.AllocatorInstance = (char*)UnrealMemory._FMemory_Malloc.Wrapper(strBytes.Length);
        Marshal.Copy(strBytes, 0, (nint)fstring->Data.AllocatorInstance, strBytes.Length);
        
        return fstring;
    }

    private void UObject_PostLoadSubobjectsImpl(UObjectBase* self, FObjectInstancingGraph* outerInstanceGraph)
    {
        if (Mod.Config.LogObjectsEnabled) Log.Information($"{nameof(UObject_PostLoadSubobjects)} || {ToolkitUtils.GetNativeName(self)} || {ToolkitUtils.GetNativeName(self->ClassPrivate)}");
        _UObject_PostLoadSubobjects.Hook!.OriginalFunction(self, outerInstanceGraph);
        _onObjectLoaded?.Invoke(new(self)); // Hopefully fine to call after original, assuming no weirdness.
    }
}