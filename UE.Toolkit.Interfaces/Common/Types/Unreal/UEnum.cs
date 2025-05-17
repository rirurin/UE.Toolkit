// ReSharper disable InvalidXmlDocComment

using System.Runtime.InteropServices;

namespace UE.Toolkit.Interfaces.Common.Types.Unreal;

[StructLayout(LayoutKind.Sequential)]
public struct UEnum
{
    public UField Super;
    public FString CppType;
    public TArray<TPair<FName, long>> Names;

    /** How the enum was originally defined. */
    //ECppForm CppForm;

    /** Enum flags. */
    //EEnumFlags EnumFlags;

    /** pointer to function used to look up the enum's display name. Currently only assigned for UEnums generated for nativized blueprints */
    //FEnumDisplayNameFn EnumDisplayNameFn;

    /** Package name this enum was in when its names were being added to the primary list */
    //FName EnumPackage;

    /** lock to be taken when accessing AllEnumNames */
    //static FRWLock AllEnumNamesLock;

    /** global list of all value names used by all enums in memory, used for property text import */
    //static TMap<FName, TMap<FName, UEnum*> > AllEnumNames;
}