using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x200)]
public unsafe struct UClass
{
    private static readonly Dictionary<string, EClassCastFlags> CastFlagsMap
        = Enum.GetValues<EClassCastFlags>().ToDictionary(x => x.ToString(), x => x);

    private static readonly Dictionary<CacheKey, bool> CachedResults = [];
        
    public UStruct Super;
    public nint ClassConstructor;
    public nint ClassVTableHelperCtorCaller;
    public nint CppClassStaticFunctions;
    public int ClassUnique;
    public int FirstOwnedClassRep;
    public bool bCooked;
    public bool bLayoutChanging;
    public EClassFlags ClassFlags;
    public EClassCastFlags ClassCastFlags;
    public UClass* ClassWithin;
    //public UObjectBase* ClassGeneratedBy; // WITH_EDITORONLY_DATA
    //public FField* PropertiesPendingDestruction; // WITH_EDITORONLY_DATA
    public FName ClassConfigName;
    public TArray<FRepRecord> ClassReps;
    public TArray<UField> NetFields;
    public UObjectBase* ClassDefaultObject;

    public readonly UClass* GetSuperClass() => (UClass*)Super.SuperStruct;
    
    public readonly bool IsChildOf(string type)
    {
        if (CastFlagsMap.TryGetValue(type, out var castFlag))
        {
            return (ClassCastFlags & castFlag) == castFlag;
        }

        // Caching might use quite a bit of memory, but I'll check that later... probably...
        var key = new CacheKey(Super.Super.Super.NamePrivate, type);
        
        if (CachedResults.TryGetValue(key, out var prevResult)) return prevResult;
        
        var result = Super.IsChildOf(type);
        CachedResults[key] = result;
        return result;
    }
    
    public readonly bool IsChildOf<T>() => IsChildOf(typeof(T).Name);

    public readonly bool IsA(string type) => IsChildOf(type);

    public readonly bool IsA<T>() => IsChildOf(typeof(T).Name);

    private readonly record struct CacheKey(FName ClassFName, string OtherClassName);
}