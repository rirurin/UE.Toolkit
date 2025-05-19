using System.Runtime.InteropServices;

namespace UE.Toolkit.Core.Types.Unreal;

[StructLayout(LayoutKind.Sequential, Size = 0x200)]
public unsafe struct UClass
{
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
    
    public readonly bool IsA(string type) => Super.IsChildOf(type);
    
    public readonly bool IsChildOf(string type) => Super.IsChildOf(type);
    
    public readonly bool IsChildOf<T>() => IsChildOf(typeof(T).Name);

    public readonly bool IsA<T>() => IsChildOf(typeof(T).Name);
}