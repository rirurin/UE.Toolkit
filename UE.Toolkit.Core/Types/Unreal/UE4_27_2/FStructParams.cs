using System.Runtime.InteropServices;
using EObjectFlags = UE.Toolkit.Core.Types.Unreal.UE5_4_4.EObjectFlags;

namespace UE.Toolkit.Core.Types.Unreal.UE4_27_2;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FStructParams
{
    public nint OuterFunc; // UObject* (*OuterFunc)(); - ZConstruct for UPackage
    public nint SuperFunc; // UScriptStruct* (*OuterFunc)();
    public nint StructOpsFunc; // UScriptStruct::ICppStructOps* (*StructOpsFunc)();
    public nint NameUTF8;
    public ulong SizeOf;
    public ulong AlignOf;
    public FPropertyParamsBase** Properties;
    public int NumProperties;
    public EObjectFlags ObjectFlags;
    public int StructFlags;

    public FPropertyParamsBase* GetProperty(int Index)
    {
        if (Index < 0 || Index >= NumProperties) return null;
        return Properties[Index];
    }
}