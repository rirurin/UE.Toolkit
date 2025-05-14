using System.Runtime.InteropServices;

namespace UE.Toolkit.Reloaded.Common.Types.Unreal;

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
}